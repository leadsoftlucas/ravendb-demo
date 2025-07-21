using LeadSoft.Common.GlobalDomain.Entities.Validations;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Orders
{
    public partial class Order : AsValidCollection
    {
        public Guid GuestId { get; set; }
        public IList<OrderItem> Items { get; set; } = [];
        public decimal ItemCount { get => Items.Sum(item => item.Quantity); }
        public decimal TotalPrice { get => Items.Sum(item => item.TotalPrice); }
    }
}
