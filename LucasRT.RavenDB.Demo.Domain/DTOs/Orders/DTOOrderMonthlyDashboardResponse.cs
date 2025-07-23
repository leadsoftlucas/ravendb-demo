using LucasRT.RavenDB.Demo.Domain.Entities.Guests;
using LucasRT.RavenDB.Demo.Domain.Entities.Orders;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace LucasRT.RavenDB.Demo.Domain.DTOs.Orders
{
    [Serializable]
    [DataContract]
    public partial class DTOOrderMonthlyDashboardResponse(int aYear, Month aMonth, IList<Order> aOrders, IList<Guest> aGuests)
    {
        [DataMember]
        public int Year { get; set; } = aYear;

        [DataMember]
        public Month Month { get; set; } = aMonth;

        [DataMember]
        public string Competency { get => string.Join('/', Month, Year); }

        [DataMember]
        public int TotalOrders { get; set; } = aOrders.Count;

        [DataMember]
        public decimal TotalItems { get; set; } = aOrders.Sum(o => o.Items.Sum(i => i.Quantity));

        [DataMember]
        public decimal TotalCost { get; set; } = aOrders.Sum(o => o.Items.Sum(i => i.TotalCost));

        [DataMember]
        public decimal TotalRevenue { get; set; } = aOrders.Sum(o => o.Items.Sum(i => i.TotalPrice));

        [DataMember]
        public decimal TotalTaxes { get => TotalRevenue * 0.1m; }

        [DataMember]
        public string Total_Cost { get => TotalCost.ToString("C"); }

        [DataMember]
        public string Total_Taxes { get => TotalTaxes.ToString("C"); }

        [DataMember]
        public string Total_Revenue { get => TotalRevenue.ToString("C"); }

        [DataMember]
        public string Profit { get => (TotalRevenue - TotalTaxes - TotalCost).ToString("C"); }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DTOBeverage MostSoldBeverage { get; set; } = aOrders.SelectMany(o => o.Items)
                                                                   .GroupBy(i => new { i.Beverage.Guid.Value, i.Beverage })
                                                                   .OrderByDescending(g => g.Sum(i => i.Quantity))
                                                                    .SelectMany(g => g.Select(o => DTOBeverage.From(o.Beverage).SetQuantity(o.Quantity)))
                                                                   .FirstOrDefault();

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DTOBeverage LessSoldBeverage { get; set; } = aOrders.SelectMany(o => o.Items)
                                                                   .GroupBy(i => i.Beverage.Guid.Value)
                                                                   .OrderBy(g => g.Sum(i => i.Quantity))
                                                                   .SelectMany(g => g.Select(o => DTOBeverage.From(o.Beverage).SetQuantity(o.Quantity)))
                                                                   .FirstOrDefault();

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DTOGuest MostRegularBuyer { get; set; } = aGuests.FirstOrDefault(g => g.Guid.Value == aOrders.GroupBy(o => o.GuestId)
                                                                                                            .OrderByDescending(g => g.Count())
                                                                                                            .FirstOrDefault()?.Key);

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DTOGuest MostSponsorBuyer { get; set; } = aGuests.FirstOrDefault(g => g.Guid.Value == aOrders.GroupBy(o => o.GuestId)
                                                                                                            .OrderByDescending(g => g.Max(o => o.TotalPrice))
                                                                                                            .FirstOrDefault()?.Key);
    }
}
