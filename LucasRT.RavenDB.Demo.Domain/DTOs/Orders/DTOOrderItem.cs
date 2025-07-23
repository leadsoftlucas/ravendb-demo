using LeadSoft.Common.GlobalDomain.DTOs.Pricing;
using LucasRT.RavenDB.Demo.Domain.Entities.Orders;
using System.Runtime.Serialization;

namespace LucasRT.RavenDB.Demo.Domain.DTOs.Orders
{
    [Serializable]
    [DataContract]
    public partial class DTOOrderItem
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public DTOBeverage Beverage { get; set; } = new();

        [DataMember]
        public DTOPrice Price { get; set; } = new();

        [DataMember]
        public decimal Quantity { get; set; } = 1;

        [DataMember]
        public decimal TotalCost { get => Price.Cost * Quantity; }

        [DataMember]
        public decimal TotalPrice { get => Price.Value * Quantity; }

        public static implicit operator DTOOrderItem(OrderItem aOrderItem)
        {
            if (aOrderItem is null)
                return null;

            return new()
            {
                Id = aOrderItem.Id,
                Beverage = aOrderItem.Beverage,
                Price = DTOPrice.ToDTO(aOrderItem.Price),
                Quantity = aOrderItem.Quantity
            };
        }
    }
}
