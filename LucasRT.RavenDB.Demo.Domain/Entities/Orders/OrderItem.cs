using LeadSoft.Common.GlobalDomain.Entities.Pricing;
using LeadSoft.Common.GlobalDomain.Entities.Validations;
using LucasRT.RavenDB.Demo.Domain.Entities.Menus;
using System.ComponentModel.DataAnnotations;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Orders
{
    public partial class OrderItem : AsValid
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Beverage Beverage { get; set; } = new();

        [Required]
        public Price Price { get; set; } = new();

        [Required]
        [Range(1.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Quantity { get; set; } = 1;

        public decimal TotalPrice { get => Price.Value * Quantity; }
    }
}
