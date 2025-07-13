using LeadSoft.Adapter.OpenAI_Bridge;
using LeadSoft.Adapter.OpenAI_Bridge.DTOs;
using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.Demo.Application.Interfaces.Guests;
using LucasRT.RavenDB.Demo.Domain.Entities.Chats;
using LucasRT.RavenDB.Demo.Domain.Entities.Guests;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Text.RegularExpressions;

namespace LucasRT.RavenDB.Demo.Application.RavenDB_Services.Guests
{
    public class GuestsService(IDocumentStore ravenDB, IOpen_AI openAI) : IGuestsService
    {
        public async Task<DTOChatHistory> GetAIContext()
        {
            Guest guest = new();

            DTOChatHistory chatHistory = await openAI.CreateChatHistoryAsync(guest);

            using IAsyncDocumentSession session = ravenDB.OpenAsyncSession();

            Chat chat = new()
            {
                Guid = chatHistory.Id,
                Context = chatHistory.Context
            };

            chat.Enable();

            await session.StoreAsync(chat);
            await session.SaveChangesAsync();

            return chatHistory;
        }

        public async Task<DTOChatHistory> FeedAIContext(Guid aId, string aMessage)
        {
            using IAsyncDocumentSession session = ravenDB.OpenAsyncSession();

            Chat chat = await session.LoadAsync<Chat>(aId.GetString());

            DTOContext[] contexts = chat.IsNotNull()
                                        ? chat.Context.ToJson().JsonToObject<DTOContext[]>()
                                        : null;

            DTOChatHistory chatHistory = await openAI.EnrichDataAsync(aId, aMessage, contexts);

            chat.Context = chatHistory.Context;
            chat.Update();

            if (chatHistory.LastMessage.ToUpper().Contains("|TRUE|"))
            {
                var guestMatch = Regex.Match(chatHistory.LastMessage, @"```json\s*\n(.*?)```", RegexOptions.Singleline);
                var messageMatch = Regex.Match(chatHistory.LastMessage, @"```plaintext\s*\n(.*?)```", RegexOptions.Singleline);

                if (guestMatch.Success)
                {
                    string json = guestMatch.Groups[1].Value.Trim();

                    Guest guest = json.JsonToObject<Guest>();
                    guest.Enable().NewId();

                    chat.GuestId = guest.Guid.Value;

                    await session.StoreAsync(guest);
                }

                if (messageMatch.Success)
                    chatHistory.LastMessage = messageMatch.Groups[1].Value.Trim();
            }

            await session.StoreAsync(chat);

            await session.SaveChangesAsync();

            return chatHistory;
        }
    }
}
