using LeadSoft.Common.GlobalDomain.DTOs;
using LucasRT.RavenDB.Demo.Domain.Entities.Orders;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LucasRT.RavenDB.Demo.Domain.DTOs.Orders
{
    [Serializable]
    [DataContract]
    public partial class DTOOrderResponse : DTOResponse
    {
        [DataMember]
        public Guid GuestId { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DTOGuest? Guest { get; set; } = null;

        [DataMember]
        public IList<DTOOrderItem> Items { get; set; } = [];

        [DataMember]
        public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

        [DataMember]
        public decimal ItemCount { get => Items.Sum(item => item.Quantity); }

        [DataMember]
        public decimal TotalCost { get => Items.Sum(item => item.TotalCost); }

        [DataMember]
        public decimal TotalPrice { get => Items.Sum(item => item.TotalPrice); }

        public static implicit operator DTOOrderResponse(Order aOrder)
        {
            if (aOrder is null)
                return null;

            return new()
            {
                Id = aOrder.Guid.Value,
                GuestId = aOrder.GuestId,
                PurchasedAt = aOrder.PurchasedAt,
                CreatedAt = aOrder.CreatedAt,
                IsEnabled = aOrder.IsEnabled,
                Items = [.. aOrder.Items.Select(i => (DTOOrderItem)i)],
            };
        }
    }
}
