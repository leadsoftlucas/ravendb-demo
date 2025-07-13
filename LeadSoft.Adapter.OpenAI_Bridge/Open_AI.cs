using LeadSoft.Adapter.OpenAI_Bridge.DTOs;
using LeadSoft.Common.Library.Extensions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

using System.Collections.Concurrent;
using static LeadSoft.Adapter.OpenAI_Bridge.Open_AI.Enums;

namespace LeadSoft.Adapter.OpenAI_Bridge
{
    public partial class Open_AI : IOpen_AI
    {
        private readonly IChatCompletionService _ChatCompletition;

        private ConcurrentDictionary<Guid, DTOChatHistory> _ActiveChatHistories = new();

        public Open_AI()
        {
            _ChatCompletition = new OpenAIChatCompletionService(OpenAI_Credential.Model_GPT_4o_mini, OpenAI_Credential.ApiKey, OpenAI_Credential.Organization);
        }

        public async Task<DTOChatHistory> CreateChatHistoryAsync(object? sampleObject = null)
        {
            try
            {
                DTOChatHistory dtoChatHistory = new();

                string sample = sampleObject is not null
                                    ? $"```json{Environment.NewLine}{sampleObject.ToJson()}{Environment.NewLine}```"
                                    : "Create the json object yourself dinamically to register a Guest and his information into NoSQL RavenDB database.";

                dtoChatHistory.Context.AddSystemMessage(_WelcomeMessage.Fill(DateTime.Now.ToString("dd-MM-yyyy hh:mm"), sample));

                ChatMessageContent chat = await _ChatCompletition.GetChatMessageContentAsync(dtoChatHistory.Context);
                dtoChatHistory.Context.Add(chat);

                _ActiveChatHistories.TryAdd(dtoChatHistory.Id, dtoChatHistory.SetMessage(chat.ToString()));

                return dtoChatHistory;
            }
            finally
            {
                GarbageCollectExpiredHistories();
            }
        }

        public async Task<DTOChatHistory> EnrichDataAsync(Guid aChatId, string aMessage, IList<DTOContext> aContexts = null)
        {
            try
            {
                _ActiveChatHistories.TryGetValue(aChatId, out DTOChatHistory dtoChatHistory);

                if (dtoChatHistory is null && aContexts is not null)
                {
                    if (dtoChatHistory is null)
                    {
                        dtoChatHistory = new()
                        {
                            Id = aChatId,
                        };
                    }

                    foreach (var item in aContexts)
                    {
                        if (item.Role.Label.ToUpper().Equals(ContextLabel.ASSISTANT.ToString()))
                            dtoChatHistory.Context.AddAssistantMessage(item.Content);
                        else if (item.Role.Label.ToUpper().Equals(ContextLabel.USER.ToString()))
                            dtoChatHistory.Context.AddUserMessage(item.Content);
                        else if (item.Role.Label.ToUpper().Equals(ContextLabel.SYSTEM.ToString()))
                            dtoChatHistory.Context.AddSystemMessage(item.Content);
                    }

                    if (dtoChatHistory is null)
                        _ActiveChatHistories.TryAdd(aChatId, dtoChatHistory);
                }

                dtoChatHistory.Context.AddUserMessage(aMessage);
                dtoChatHistory.Context.AddSystemMessage(_EnrichDataMessage);

                ChatMessageContent chat = await _ChatCompletition.GetChatMessageContentAsync(dtoChatHistory.Context);

                dtoChatHistory.Context.Add(chat);

                _ActiveChatHistories.TryAdd(dtoChatHistory.Id, dtoChatHistory.SetMessage(chat.ToString()));

                return dtoChatHistory;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                GarbageCollectExpiredHistories();
            }
        }

        private void GarbageCollectExpiredHistories()
        {
            var expiredHistories = _ActiveChatHistories.Where(kvp => kvp.Value.HasExpired)
                                                       .Select(kvp => kvp.Key);

            foreach (var historyId in expiredHistories)
                _ActiveChatHistories.TryRemove(historyId, out _);
        }

        public void Dispose()
        {
            _ActiveChatHistories.Clear();
            _ActiveChatHistories = null!;
        }
    }
}
