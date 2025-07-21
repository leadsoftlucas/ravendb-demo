using LeadSoft.Common.GlobalDomain.DTOs;
using LucasRT.RavenDB.Demo.Domain.Entities.Guests;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LucasRT.RavenDB.Demo.Domain.DTOs.Guests
{
    [Serializable]
    [DataContract]
    public class DTOGuestSearchResponse : DTOResponse
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        public string Nationality { get; set; } = string.Empty;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Other_Relevant_Information { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<DTOSocialNetwork> SocialNetworks { get; set; } = [];

        public static implicit operator DTOGuestSearchResponse(Guest guest)
        {
            if (guest is null)
                return null;

            return new()
            {
                Id = guest.Guid.Value,
                Name = guest.Name,
                Email = guest.Email,
                Nationality = guest.Nationality,
                Other_Relevant_Information = guest.Other_Relevant_Information,
                SocialNetworks = guest.SocialNetworks?.Select(sn => (DTOSocialNetwork)sn)
            };
        }
    }
}
