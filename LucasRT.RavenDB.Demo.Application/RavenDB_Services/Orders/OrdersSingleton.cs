using LucasRT.RavenDB.Demo.Application.Interfaces.Menus;
using Microsoft.Extensions.Configuration;

namespace LucasRT.RavenDB.Demo.Application.RavenDB_Services.Orders
{
    public partial class OrdersSingleton(IConfiguration configuration) : IOrdersSingleton
    {
        private bool disposedValue;

        public async Task StartAttendanceAsync()
        {
            if (_IsOpen)
                return;

            _IsOpen = true;

            FillListsAsync().Wait();

            await BulkInsertAsChannelAsync();
        }

        public async Task StopAttendanceAsync()
        {
            _IsOpen = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _RavenDBSession.Dispose();
                }

                _ChannelProducerTasks.Clear();
                _ChannelProducerTasks = null;

                _ChannelWriter = null;
                _ChannelReader = null;

                _Beverages.Clear();
                _Beverages = null;

                _Guests.Clear();
                _Guests = null;

                _RavenDB.Dispose();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
