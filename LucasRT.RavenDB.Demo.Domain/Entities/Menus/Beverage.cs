using LeadSoft.Common.GlobalDomain.Entities.Validations;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static LucasRT.RavenDB.Demo.Domain.Entities.Menus.Enums;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Menus
{
    /// <summary>
    /// Beverage entity representing a wine, beer or whisky to feed the menu.
    /// </summary>
    public partial class Beverage : AsValidCollection
    {
        /// <summary>
        /// Beverage label title (name of the product).
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Wine, Beer, Whisky
        /// </summary>
        [Required]
        public BeverageType BeverageType { get; set; }

        /// <summary>
        /// A brief description of the beverage, such as tasting notes or characteristics. A little about the origin and the producer, what makes it special and how it can be enjoyed. Also, it can include food pairing suggestions.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// WineType, BeerStyle, or WhiskyType — e.g., Red, IPA, Single Malt
        /// </summary>
        [Required]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Grape for wine, Distilled grain for whisky, or left null for beer
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Variety { get; set; } = null;

        /// <summary>
        /// Winery, Brewery, or Distillery
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Producer { get; set; } = null;

        /// <summary>
        /// Year of harvest or aging, relevant mostly for wine/whisky
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Vintage { get; set; } = null;

        /// <summary>
        /// Beer-specific (bitterness), nullable otherwise
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? IBU { get; set; } = null;

        /// <summary>
        /// Alcohol by volume percentage, applicable to all beverages
        /// </summary>
        [Required]
        [Range(0, 100)]
        public double AlcoholPercentage { get; set; }

        /// <summary>
        /// Origin country of the beverage, such as France, USA, Scotland, etc.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Country { get; set; } = string.Empty;

        public string VectorSearchField { get; private set; }
    }

    public static partial class Enums
    {
        public enum BeverageType
        {
            [Description("Beer")]
            Beer,
            [Description("Wine")]
            Wine,
            [Description("Whisky")]
            Whisky
        }
    }
}