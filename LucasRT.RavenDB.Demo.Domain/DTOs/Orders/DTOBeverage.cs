using LucasRT.RavenDB.Demo.Domain.Entities.Menus;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using static LucasRT.RavenDB.Demo.Domain.Entities.Menus.Enums;

namespace LucasRT.RavenDB.Demo.Domain.DTOs.Orders
{
    [Serializable]
    [DataContract]
    public partial class DTOBeverage
    {
        /// <summary>
        /// Item Id
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Composed item label title (name of the product).
        /// </summary>
        [DataMember]
        public string Item { get; set; } = string.Empty;

        /// <summary>
        /// Beverage type of the item, such as Wine, Beer, or Whisky.
        /// </summary>
        [DataMember]
        public BeverageType BeverageType { get; set; }

        /// <summary>
        /// Description of the item, such as tasting notes or characteristics. A little about the origin and the producer, what makes it special and how it can be enjoyed. Also, it can include food pairing suggestions.
        /// </summary>
        [DataMember]
        public string Description { get; set; } = string.Empty;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? QuantitySold { get; set; } = null;

        public DTOBeverage SetQuantity(decimal aQuantity)
        {
            QuantitySold = aQuantity;
            return this;
        }

        public static DTOBeverage From(Beverage aBeverage)
            => aBeverage;

        public static implicit operator DTOBeverage(Beverage aBeverage)
        {
            if (aBeverage is null)
                return null;

            return new()
            {
                Id = aBeverage.Guid.Value,
                Item = string.Join(" | ", aBeverage.Label,
                                          aBeverage.Country,
                                          aBeverage.Type,
                                          aBeverage.Variety,
                                          aBeverage.Producer),
                BeverageType = aBeverage.BeverageType,
                Description = aBeverage.Description
            };
        }
    }
}
