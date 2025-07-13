using LeadSoft.Adapter.OpenAI_Bridge.DTOs;
using LeadSoft.Common.Library.Extensions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

using System.Collections.Concurrent;

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

                dtoChatHistory.Context.AddSystemMessage($@"You are a helpful assistant and your first task is to request users information ask him to introduce himself. So this is your first question to him to open the conversation.
                                                           You work in a RDB Beverage Database and want to know him better to fill a form with the following sample field getting info from the conversation.
                                                           When he replies, you must generate this object for us to Store in database.
                                                           ```json {sampleObject.ToJson()} ```");

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
                        if (item.Role.Label.ToUpper() == "ASSISTANT")
                            dtoChatHistory.Context.AddAssistantMessage(item.Content);
                        else if (item.Role.Label.ToUpper() == "USER")
                            dtoChatHistory.Context.AddUserMessage(item.Content);
                        else if (item.Role.Label.ToUpper() == "SYSTEM")
                            dtoChatHistory.Context.AddSystemMessage(item.Content);
                    }

                    if (dtoChatHistory is null)
                        _ActiveChatHistories.TryAdd(aChatId, dtoChatHistory);
                }

                dtoChatHistory.Context.AddUserMessage(aMessage);
                dtoChatHistory.Context.AddSystemMessage(@"Now you will use this data to understand was told to you and you must extract the information and return a message in two different flows:
                                                          1. All information is filled and you can give us the full data and thank the guest, welcoming him to find some Beverage of his taste and add the text '|true|' at the end of the message.
                                                          2. If some personal information required on object is missing, ask for it and add the text '|false|' at the end of the message, but ignore regular database fields (ids, creation date, valid objects, etc).
                                                          Using markdown as 'json' code snippet, use this information to fill object template I sent you and if you get any other interesting information, create a structured and organized object with separated properties and fill it in 'Other_Relevant_Information' property as a new object.
                                                          Add another separated markdown code snippet as 'plaintext' to return me the a good response message direct to the Guest, informing that his registry is completed.");

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
