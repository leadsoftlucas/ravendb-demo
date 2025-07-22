using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.Demo.Domain.Entities.Guests;
using LucasRT.RavenDB.Demo.Domain.Entities.Menus;
using LucasRT.RavenDB.Demo.Domain.Entities.Orders;
using Raven.Client.Documents.BulkInsert;
using System.Threading.Channels;

namespace LucasRT.RavenDB.Demo.Application.RavenDB_Services.Orders
{
    public partial class OrdersSingleton
    {
        /// <summary>
        /// Represents a collection of tasks that are responsible for producing items to a channel.
        /// </summary>
        /// <remarks>This field is intended for internal use and stores the tasks that manage the
        /// production of items to a channel. It is not exposed publicly and should be used only within the context of
        /// the class implementation.</remarks>
        private IList<Task> _ChannelProducerTasks = [];

        /// <summary>
        /// Represents the writer side of a channel used for sending objects.
        /// </summary>
        /// <remarks>This field is intended for internal use and provides access to the channel writer for
        /// sending data to the associated channel. It is not exposed publicly.</remarks>
        private ChannelWriter<Order> _ChannelWriter = null;
        private ChannelReader<Order> _ChannelReader = null;

        public async Task BulkInsertAsChannelAsync()
        {
            if (!_IsOpen || !_Guests.Any() || !_Beverages.Any())
                return;

            _ChannelProducerTasks = [];

            Channel<Order> channel = Channel.CreateBounded<Order>(new BoundedChannelOptions(capacity: 5000)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = false
            });

            _ChannelWriter = channel.Writer;
            _ChannelReader = channel.Reader;

            Task consumerTask = Task.Run(async () =>
            {
                using BulkInsertOperation bulkInsert = _RavenDB.BulkInsert(new BulkInsertOptions { SkipOverwriteIfUnchanged = true });
                try
                {
                    while (await _ChannelReader.WaitToReadAsync().ConfigureAwait(false))
                        while (_ChannelReader.TryRead(out Order order))
                        {
                            await bulkInsert.StoreAsync(order, order.Id)
                                            .ConfigureAwait(false);

                            if (!order.IsInvalid)
                            {
                                await bulkInsert.CountersFor(order.GuestId.GetString())
                                                .IncrementAsync(Guest.VisitsCounterName, 1)
                                                .ConfigureAwait(false);

                                foreach (OrderItem orderItem in order.Items)
                                {
                                    await bulkInsert.CountersFor(orderItem.Beverage.Id)
                                                    .IncrementAsync(Beverage.PurchasesCounterName, 1)
                                                    .ConfigureAwait(false);

                                    await bulkInsert.CountersFor(orderItem.Beverage.Id)
                                                    .IncrementAsync(Beverage.SoldCounterName, (int)orderItem.Quantity)
                                                    .ConfigureAwait(false);
                                }
                            }
                        }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro no consumidor (inserção em massa): {ex.Message}");
                    throw;
                }
            });

            ProduceOrders();

            Exception producerError = null;
            try
            {
                await Task.WhenAll(_ChannelProducerTasks).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                producerError = ex;
                Console.WriteLine($"Failed while producing data: {ex.Message}");
            }
            finally
            {
                _ChannelWriter.Complete(producerError);
            }

            try
            {
                await consumerTask.ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }

            if (producerError.IsNotNull())
                throw producerError;
        }

        private void ProduceOrders()
        {
            if (!_IsOpen || !_Guests.Any() || !_Beverages.Any())
                return;

            Random random = new();

            _ChannelProducerTasks.Add(Task.Run(async () =>
            {
                try
                {
                    while (_IsOpen)
                    {
                        KeyValuePair<Guid, Guest> guest = _Guests.Skip(random.Next(0, _Guests.Count())).Take(1).FirstOrDefault();
                        Order order = new Order().Randomize(guest.Value, [.. _Beverages.Values]);
                        await _ChannelWriter.WriteAsync(order).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro na produção de 'Products': {ex.Message}");
                    throw;
                }
            }));
        }
    }
}
