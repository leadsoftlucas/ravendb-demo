using System.ComponentModel.DataAnnotations;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Guests
{
    public class SocialNetwork
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string Url { get; set; } = string.Empty;
    }
}
