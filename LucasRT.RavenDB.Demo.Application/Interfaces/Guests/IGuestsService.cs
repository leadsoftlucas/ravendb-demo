using LeadSoft.Adapter.OpenAI_Bridge.DTOs;
using LucasRT.RavenDB.Demo.Domain.DTOs.Guests;

namespace LucasRT.RavenDB.Demo.Application.Interfaces.Guests
{
    public interface IGuestsService
    {
        Task<DTOGuestResponse> LoadAsync(Guid aId);
        Task<DTOGuestResponse> CreateAsync(DTOGuestRequest dto);
        Task<DTOChatHistory> GetAIContext();
        Task<DTOChatHistory> FeedAIContext(Guid aId, string aMessage);
    }
}
