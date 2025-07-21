using LeadSoft.Adapter.OpenAI_Bridge.DTOs;
using LucasRT.RavenDB.Demo.Domain.DTOs;
using LucasRT.RavenDB.Demo.Domain.DTOs.Guests;

namespace LucasRT.RavenDB.Demo.Application.Interfaces.Guests
{
    public interface IGuestsService
    {
        Task<DTOGuestResponse> LoadAsync(Guid aId);
        Task<IList<DTOGuestSearchResponse>> SearchAsync(string aSearch = "", int currentPage = 0);
        Task<IList<DTOGuestSearchResponse>> VectorSearchAsync(string aSearch, int currentPage = 0);
        Task<DTOGuestResponse> CreateAsync(DTOGuestRequest dto);
        Task<DTOChatHistory> GetAIContext();
        Task<DTOChatHistory> FeedAIContext(Guid aId, string aMessage);
        Task<DTOOperationStatisticsResponse> CreateDataBulkInsertAsync();
    }
}
