using LucasRT.RavenDB.Demo.Domain.Entities.Guests;
using LucasRT.RavenDB.Demo.Domain.Entities.Menus;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Orders
{
    public partial class Order
    {
        public Order()
        {
        }

        public Order Randomize(Guest guest, IList<Beverage> beverages)
        {
            Create();

            Random random = new();
            DateTime start = new(DateTime.UtcNow.Year, 1, 1);

            GuestId = guest.Guid.Value;
            PurchasedAt = start.AddDays(random.Next(((DateTime.Today - (start))).Days));
            Items = [.. Enumerable.Range(0, random.Next(1, 5)).Select(_ => new OrderItem().Randomize().SetBeverage(beverages.Skip(random.Next(0, beverages.Count())).Take(1).First()))];

            Validate();

            return this;
        }


    }
}
