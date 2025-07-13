using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Guests
{
    [Serializable]
    [DataContract]
    [Table("SocialNetwork", Schema = "add")]
    public class SocialNetwork
    {
        [Key]
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;

        [JsonIgnore]
        public string GuestId { get; set; } = string.Empty;

        [Required]
        [DataMember]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [DataMember]
        public string Url { get; set; } = string.Empty;
    }
}
