using LucasRT.RavenDB.Demo.Domain.Entities.Menus;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using static LucasRT.RavenDB.Demo.Domain.Entities.Menus.Enums;

namespace LucasRT.RavenDB.Demo.Domain.DTOs.Menus
{
    [Serializable]
    [DataContract]
    public partial class DTOMenuResponse
    {
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Beverage label title (name of the product).
        /// </summary>
        [DataMember]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Wine, Beer, Whisky
        /// </summary>
        [DataMember]
        public BeverageType BeverageType { get; set; }

        /// <summary>
        /// A brief description of the beverage, such as tasting notes or characteristics. A little about the origin and the producer, what makes it special and how it can be enjoyed. Also, it can include food pairing suggestions.
        /// </summary>
        [DataMember]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// WineType, BeerStyle, or WhiskyType — e.g., Red, IPA, Single Malt
        /// </summary>
        [DataMember]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Grape for wine, Distilled grain for whisky, or left null for beer
        /// </summary>
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Variety { get; set; } = null;

        /// <summary>
        /// Winery, Brewery, or Distillery
        /// </summary>
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Producer { get; set; } = null;

        /// <summary>
        /// Year of harvest or aging, relevant mostly for wine/whisky
        /// </summary>
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Vintage { get; set; } = null;

        /// <summary>
        /// Beer-specific (bitterness), nullable otherwise
        /// </summary>
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? IBU { get; set; } = null;

        /// <summary>
        /// Alcohol by volume percentage, applicable to all beverages
        /// </summary>
        [DataMember]
        public double AlcoholPercentage { get; set; }

        /// <summary>
        /// Origin country of the beverage, such as France, USA, Scotland, etc.
        /// </summary>
        [DataMember]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Dislikes counter for the beverage.
        /// </summary>
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? Dislikes { get; private set; } = null;

        /// <summary>
        /// Likes counter for the beverage.
        /// </summary>
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? Likes { get; private set; } = null;

        public DTOMenuResponse SetLikes(long? likes)
        {
            Likes = likes;
            return this;
        }

        public DTOMenuResponse SetDislikes(long? dislikes)
        {
            Dislikes = dislikes;
            return this;
        }

        public static implicit operator DTOMenuResponse(Beverage Beverage)
        {
            return new DTOMenuResponse
            {
                Id = Beverage.Guid.Value,
                Label = Beverage.Label,
                AlcoholPercentage = Beverage.AlcoholPercentage,
                BeverageType = Beverage.BeverageType,
                Type = Beverage.Type,
                Variety = Beverage.Variety,
                IBU = Beverage.IBU,
                Producer = Beverage.Producer,
                Vintage = Beverage.Vintage,
                Description = Beverage.Description,
                Country = Beverage.Country
            };
        }
    }
}
