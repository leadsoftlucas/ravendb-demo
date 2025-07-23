using System.Runtime.Serialization;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace LucasRT.RavenDB.Demo.Domain.DTOs.Orders
{
    [Serializable]
    [DataContract]
    public partial class DTOOrderAnnualDashboardResponse
    {
        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public int Month { get; set; }

        [DataMember]
        private Month CurrentMonth { get => GetByValue<Month>(Month); }

        [DataMember]
        public string Competency { get => string.Join('/', CurrentMonth, Year); }

        [DataMember]
        public int TotalOrders { get; set; }

        [DataMember]
        public decimal TotalItems { get; set; }

        [DataMember]
        public decimal TotalCost { get; set; }

        [DataMember]
        public decimal TotalRevenue { get; set; }

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
    }
}
