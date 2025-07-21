using LucasRT.RavenDB.Demo.Domain.Entities.Menus;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Orders
{
    public partial class Order
    {
        public Order()
        {
        }

        public Order Randomize(IList<Beverage> beverages)
        {
            var random = new Random();
            Items = [.. Enumerable.Range(0, random.Next(1, 5)).Select(_ => new OrderItem().Randomize().SetBeverage(beverages.Skip(random.Next(0, beverages.Count())).Take(1).First()))];
            return this;
        }


    }
}
