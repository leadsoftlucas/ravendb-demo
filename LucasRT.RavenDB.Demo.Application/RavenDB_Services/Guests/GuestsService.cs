using LeadSoft.Adapter.OpenAI_Bridge;
using LeadSoft.Adapter.OpenAI_Bridge.DTOs;
using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.Demo.Application.Interfaces.Guests;
using LucasRT.RavenDB.Demo.Domain.DTOs;
using LucasRT.RavenDB.Demo.Domain.DTOs.Guests;
using LucasRT.RavenDB.Demo.Domain.Entities.Chats;
using LucasRT.RavenDB.Demo.Domain.Entities.Guests;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace LucasRT.RavenDB.Demo.Application.RavenDB_Services.Guests
{
    /// <summary>
    /// Demo - up to 10 minutes
    /// Querying & indexing in ravendb - intent to blow our mind
    /// focus on speed of development and the performance at runtime
    /// overall complexity of the solution - contrast with other alternatives and show how we do better
    /// </summary>
    public class GuestsService(IDocumentStore ravenDB, IOpen_AI openAI) : IGuestsService
    {
        public async Task<DTOGuestResponse> LoadAsync(Guid aId)
        {
            using IAsyncDocumentSession ravendbsession = ravenDB.OpenAsyncSession();

            return await ravendbsession.LoadAsync<Guest>(aId.GetString());
        }

        public async Task<IList<DTOGuestSearchResponse>> SearchAsync(string aSearch = "", int currentPage = 0)
        {
            using IAsyncDocumentSession ravendbsession = ravenDB.OpenAsyncSession();

            IRavenQueryable<Guest> query;

            if (aSearch.IsNothing())
                query = ravendbsession.Query<Guest>();
            else
            {
                aSearch = $"{aSearch.Trim()}*";
                query = ravendbsession.Query<Guest>()
                                      //.Search(x => x.Label, aSearch, 5)
                                      .Where(x => x.IsEnabled && !x.IsInvalid);
            }

            IList<Guest> guests = await query.Where(x => x.IsEnabled)
                                           .Skip(currentPage * 100)
                                           .Take(100)
                                           .ToListAsync();

            return [.. guests.Select(guest => (DTOGuestSearchResponse)guest)];
        }

        public async Task<IList<DTOGuestSearchResponse>> VectorSearchAsync(string aSearch, int currentPage = 0)
        {
            using IAsyncDocumentSession ravendbsession = ravenDB.OpenAsyncSession();

            IList<Guest> guests = await ravendbsession.Advanced.AsyncDocumentQuery<Guest>()
                                                                     .Skip(currentPage * 100)
                                                                     .Take(100)
                                                                     .VectorSearch(field => field.WithText(x => x.VectorSearchField),
                                                                                   searchTerm => searchTerm.ByText(aSearch))
                                                                     .WaitForNonStaleResults()
                                                                     .ToListAsync();

            return [.. guests.Select(guest => (DTOGuestSearchResponse)guest)];
        }

        public async Task<DTOGuestResponse> CreateAsync(DTOGuestRequest dto)
        {
            using IAsyncDocumentSession ravendbsession = ravenDB.OpenAsyncSession();

            Guest guest = dto;

            await ravendbsession.StoreAsync(guest, guest.Id);
            await ravendbsession.SaveChangesAsync();

            return guest;
        }

        public async Task<DTOChatHistory> GetAIContext()
        {
            DTOChatHistory chatHistory = await openAI.CreateChatHistoryAsync(Guest.GetSample());

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

            var messageMatch = Regex.Match(chatHistory.LastMessage, @"```plaintext\s*\n(.*?)```", RegexOptions.Singleline);

            if (chatHistory.LastMessage.ToUpper().Contains("✅"))
            {
                var guestMatch = Regex.Match(chatHistory.LastMessage, @"```json\s*\n(.*?)```", RegexOptions.Singleline);

                if (guestMatch.Success)
                {
                    string json = guestMatch.Groups[1].Value.Trim();

                    Guest guest = json.JsonParser<Guest>();
                    guest.Create();
                    guest.CreateVectorField().Validate();

                    chat.GuestId = guest.Guid.Value;
                    chatHistory.GuestId = guest.Guid;

                    await session.StoreAsync(guest);
                }
            }

            chatHistory.LastMessage = messageMatch.Success
                                            ? messageMatch.Groups[1].Value.Trim()
                                            : chatHistory.LastMessage.Replace("❌", string.Empty)
                                                                     .Replace("✅", string.Empty);

            await session.StoreAsync(chat);

            await session.SaveChangesAsync();

            return chatHistory;
        }

        public async Task<DTOOperationStatisticsResponse> CreateDataBulkInsertAsync()
        {
            DTOOperationStatisticsResponse dtoResponse = new("Creating mocked data into RavenDB with Bulk Insert");

            ConcurrentDictionary<Guid, Guest> indexedGuests = Guest.GetSamples(out DTOOperation dtoFileOperation);
            DTOOperation dtoDataPreparationOperation = PrepareDataParallel(indexedGuests);
            DTOOperation dtoBulkOperation = await BulkInsertAsync([.. indexedGuests.Values]);

            dtoResponse.DtoOperations.Add(dtoFileOperation);
            dtoResponse.DtoOperations.Add(dtoDataPreparationOperation);
            dtoResponse.DtoOperations.Add(dtoBulkOperation);

            return dtoResponse.Finish();
        }

        private static DTOOperation PrepareDataParallel(ConcurrentDictionary<Guid, Guest> guests)
        {
            DTOOperation dtoDataPreparationOperation = new($"Prepare data serializing and flatting information in Parallel Operation on {guests.Count} documents.");

            Parallel.ForEach(guests, guest =>
            {
                guest.Value.Enable();
                guest.Value.CreateVectorField().Validate();
            });

            return dtoDataPreparationOperation.Finish();
        }

        private async Task<DTOOperation> BulkInsertAsync(IList<Guest> guests)
        {
            DTOOperation dtoBulkOperation = new($"Bulk Insert Operation on {guests.Count} documents.");

            using var bulkInsert = ravenDB.BulkInsert();

            foreach (Guest guest in guests)
                await bulkInsert.StoreAsync(guest, guest.Id);

            return dtoBulkOperation.Finish();
        }
    }
}
