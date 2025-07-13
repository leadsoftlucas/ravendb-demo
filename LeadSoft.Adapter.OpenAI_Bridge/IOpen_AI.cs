using LeadSoft.Adapter.OpenAI_Bridge.DTOs;

namespace LeadSoft.Adapter.OpenAI_Bridge
{
    public interface IOpen_AI : IDisposable
    {
        Task<DTOChatHistory> CreateChatHistoryAsync(object? sampleObject = null);
        Task<DTOChatHistory> EnrichDataAsync(Guid aChatId, string aMessage, IList<DTOContext> aContexts = null);
    }
}
