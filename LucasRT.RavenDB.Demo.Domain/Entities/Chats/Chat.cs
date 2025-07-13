using LeadSoft.Common.GlobalDomain.Entities;

namespace LucasRT.RavenDB.Demo.Domain.Entities.Chats
{
    public class Chat : CollectionsBase
    {
        public Guid? GuestId { get; set; }

        public object Context { get; set; }
    }
}
