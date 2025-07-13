using LeadSoft.Adapter.OpenAI_Bridge.DTOs;

namespace LucasRT.RavenDB.Demo.Application.Interfaces.Guests
{
    public interface IGuestsService
    {
        Task<DTOChatHistory> GetAIContext();
        Task<DTOChatHistory> FeedAIContext(Guid aId, string aMessage);
    }
}
