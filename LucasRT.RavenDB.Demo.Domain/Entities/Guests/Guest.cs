using LeadSoft.Common.GlobalDomain.Entities.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Guests
{
    [Serializable]
    [DataContract]
    [Table("Guest")]
    public class Guest : AsValidCollection
    {
        [Required]
        [DataMember]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [DataMember]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataMember]
        public string Nationality { get; set; } = string.Empty;

        [Required]
        [DataMember]
        [NotMapped]
        public dynamic Other_Relevant_Information { get; set; }

        [DataMember]
        public IList<SocialNetwork> SocialNetworks { get; set; } = [];

        public static Guest GetSample()
            => new()
            {
                Name = "",
                Email = "",
                Nationality = "",
                Other_Relevant_Information = new { },
                SocialNetworks =
                [
                    new ()
                ]
            };
    }
}
