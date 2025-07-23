using LucasRT.RavenDB.Demo.Domain.Entities.Guests;
using System.Runtime.Serialization;

namespace LucasRT.RavenDB.Demo.Domain.DTOs.Orders
{
    [Serializable]
    [DataContract]
    public partial class DTOGuest
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Email { get; set; } = string.Empty;

        public static implicit operator DTOGuest(Guest aGuest)
        {
            if (aGuest is null)
                return null;

            return new()
            {
                Id = aGuest.Guid.Value,
                Name = aGuest.Name,
                Email = aGuest.Email
            };
        }
    }
}
