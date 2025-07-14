using LeadSoft.Common.GlobalDomain.Entities.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Guests
{
    [Table("Guest")]
    public partial class Guest : AsValidCollection
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Nationality { get; set; } = string.Empty;

        [Required]
        [NotMapped]
        public dynamic Other_Relevant_Information { get; set; }

        public IList<SocialNetwork> SocialNetworks { get; set; } = [];
    }
}
