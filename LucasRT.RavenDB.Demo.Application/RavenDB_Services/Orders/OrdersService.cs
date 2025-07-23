using LeadSoft.Adapter.OpenAI_Bridge;
using LucasRT.RavenDB.Demo.Application.Interfaces.Menus;
using LucasRT.RavenDB.Demo.Domain.DTOs.Orders;
using LucasRT.RavenDB.Demo.Domain.Entities.Guests;
using LucasRT.RavenDB.Demo.Domain.Entities.Orders;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace LucasRT.RavenDB.Demo.Application.RavenDB_Services.Orders
{
    /// <summary>
    /// Demo - up to 10 minutes
    /// Querying & indexing in ravendb - intent to blow our mind
    /// focus on speed of development and the performance at runtime
    /// overall complexity of the solution - contrast with other alternatives and show how we do better
    /// </summary>
    public class OrdersService(IDocumentStore ravenDB, IOpen_AI openAI) : IOrdersService
    {
        public Task<DTOOrderResponse> LoadAsync(Guid aId)
        {
            using IAsyncDocumentSession ravendbsession = ravenDB.OpenAsyncSession();

            throw new NotImplementedException();
        }

        public async Task<IList<DTOOrderAnnualDashboardResponse>> AnnualDashboardAsync(Guid? aGuestId = null, Guid? aBeverageId = null)
        {
            using IAsyncDocumentSession ravendbsession = ravenDB.OpenAsyncSession();

            IRavenQueryable<Order> query = ravendbsession.Query<Order>()
                                                         .Where(o => o.IsEnabled && !o.IsInvalid);

            if (aGuestId.HasValue)
                query = query.Where(o => o.GuestId == aGuestId.Value);

            if (aBeverageId.HasValue)
                query = query.Where(o => o.Items.Any(i => i.Beverage.Guid.Value == aBeverageId));

            return await query.GroupBy(g => new { g.IsEnabled, g.IsInvalid, g.PurchasedAt.Year, g.PurchasedAt.Month })
                              .Select(g => new DTOOrderAnnualDashboardResponse()
                              {
                                  Year = g.Key.Year,
                                  Month = g.Key.Month,
                                  TotalOrders = g.Count(),
                                  TotalItems = g.Sum(x => x.ItemCount),
                                  TotalCost = g.Sum(x => x.TotalCost),
                                  TotalRevenue = g.Sum(x => x.TotalPrice)
                              })
                              .OrderBy(o => o.Year)
                              .ThenBy(o => o.Month)
                              .ToListAsync();
        }

        public async Task<DTOOrderMonthlyDashboardResponse> MonthlyDashboardAsync(int aYear, Month aMonth, Guid? aGuestId = null, Guid? aBeverageId = null)
        {
            using IAsyncDocumentSession ravendbsession = ravenDB.OpenAsyncSession();

            IRavenQueryable<Order> query = ravendbsession.Query<Order>()
                                                         .Where(o => o.IsEnabled && !o.IsInvalid &&
                                                                     o.PurchasedAt.Year == aYear &&
                                                                     o.PurchasedAt.Month == aMonth.GetValue());

            if (aGuestId.HasValue)
                query = query.Where(g => g.GuestId == aGuestId.Value);

            if (aBeverageId.HasValue)
                query = query.Where(g => g.Items.Any(i => i.Beverage.Guid.Value == aBeverageId));

            IList<Order> orders = await query.ToListAsync();

            IList<Guid> guestGuids = orders.Select(o => o.GuestId).Distinct().ToList();

            IList<Guest> guests = await ravendbsession.Query<Guest>()
                                                      .Where(g => g.Guid.Value.In(guestGuids))
                                                      .ToListAsync();

            return new(aYear, aMonth, orders, guests);
        }
    }
}
