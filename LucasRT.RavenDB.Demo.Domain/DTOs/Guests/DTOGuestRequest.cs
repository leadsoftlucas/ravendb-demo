using LucasRT.RavenDB.Demo.Domain.Entities.Guests;
using System.Runtime.Serialization;

namespace LucasRT.RavenDB.Demo.Domain.DTOs.Guests
{
    [Serializable]
    [DataContract]
    public class DTOGuestRequest
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        public string Nationality { get; set; } = string.Empty;

        [DataMember]
        public IEnumerable<DTOSocialNetwork> SocialNetworks { get; set; } = [];

        public static implicit operator Guest(DTOGuestRequest dto)
        {
            if (dto is null)
                throw new OperationCanceledException("Guest cannot comes from a null dto.");

            Guest guest = new()
            {
                Name = dto.Name,
                Email = dto.Email,
                Nationality = dto.Nationality,
                SocialNetworks = [.. dto.SocialNetworks?.Select(sn => sn)]
            };

            guest.Create();
            guest.CreateVectorField().Validate();

            return guest;
        }
    }
}
