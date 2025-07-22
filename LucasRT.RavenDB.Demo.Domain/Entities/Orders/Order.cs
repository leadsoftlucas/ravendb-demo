using LeadSoft.Common.GlobalDomain.Entities.Validations;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Orders
{
    public partial class Order : AsValidCollection
    {
        public Guid GuestId { get; set; }
        public IList<OrderItem> Items { get; set; } = [];
        public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

        public decimal ItemCount { get => Items.Sum(item => item.Quantity); }
        public decimal TotalCost { get => Items.Sum(item => item.TotalCost); }
        public decimal TotalPrice { get => Items.Sum(item => item.TotalPrice); }
    }
}
