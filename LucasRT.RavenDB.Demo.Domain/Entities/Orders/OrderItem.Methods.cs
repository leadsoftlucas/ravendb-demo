using LeadSoft.Common.GlobalDomain.Entities.Pricing;
using LucasRT.RavenDB.Demo.Domain.Entities.Menus;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Orders
{
    public partial class OrderItem
    {
        public OrderItem Randomize()
        {
            var random = new Random();
            Id = Guid.NewGuid();
            Price = new Price((decimal)(random.NextDouble() * 100), (decimal)(random.NextDouble() * 50));
            Quantity = (decimal)(random.NextDouble() * 10 + 1);

            return this;
        }

        public OrderItem SetBeverage(Beverage beverage)
        {
            Beverage = beverage ?? throw new ArgumentNullException(nameof(beverage), "Beverage cannot be null.");
            return this;
        }
    }
}
