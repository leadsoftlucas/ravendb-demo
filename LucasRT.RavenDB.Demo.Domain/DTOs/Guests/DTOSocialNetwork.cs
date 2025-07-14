using LucasRT.RavenDB.Demo.Domain.Entities.Guests;
using System.Runtime.Serialization;

namespace LucasRT.RavenDB.Demo.Domain.DTOs.Guests
{
    [Serializable]
    [DataContract]
    public class DTOSocialNetwork
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Url { get; set; } = string.Empty;

        public static implicit operator DTOSocialNetwork(SocialNetwork socialNetwork)
        {
            if (socialNetwork is null)
                return null;

            return new DTOSocialNetwork
            {
                Name = socialNetwork.Name,
                Url = socialNetwork.Url
            };
        }

        public static implicit operator SocialNetwork(DTOSocialNetwork dto)
        {
            if (dto is null)
                return null;

            return new()
            {
                Name = dto.Name,
                Url = dto.Url
            };
        }
    }
}
