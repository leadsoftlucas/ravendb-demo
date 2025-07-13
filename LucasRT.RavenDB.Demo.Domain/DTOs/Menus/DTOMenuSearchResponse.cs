using LucasRT.RavenDB.Demo.Domain.Entities.Menus;
using System.Runtime.Serialization;
using static LucasRT.RavenDB.Demo.Domain.Entities.Menus.Enums;

namespace LucasRT.RavenDB.Demo.Domain.DTOs.Menus
{
    [Serializable]
    [DataContract]
    public partial class DTOMenuSearchResponse
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

        public static implicit operator DTOMenuSearchResponse(Beverage Beverage)
        {
            return new DTOMenuSearchResponse
            {
                Id = Beverage.Guid.Value,
                Item = string.Join(" | ", Beverage.Label,
                                          Beverage.Country,
                                          Beverage.Type,
                                          Beverage.Variety,
                                          Beverage.Producer),
                BeverageType = Beverage.BeverageType,
                Description = Beverage.Description
            };
        }
    }
}
