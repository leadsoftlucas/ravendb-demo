using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.Demo.Application.Interfaces.Menus;
using LucasRT.RavenDB.Demo.Domain.DTOs;
using LucasRT.RavenDB.Demo.Domain.DTOs.Menus;
using LucasRT.RavenDB.Demo.Domain.Entities.Menus;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace LucasRT.RavenDB.Demo.Application.RavenDB_Services.Menus
{
    public class MenusRavenDBService(IDocumentStore ravenDB) : IMenusService
    {
        public async Task<DTOMenuResponse> LoadAsync(Guid aId)
        {
            using IAsyncDocumentSession ravendbsession = ravenDB.OpenAsyncSession();

            IAsyncSessionDocumentCounters documentCounters = ravendbsession.CountersFor(aId.GetString());

            DTOMenuResponse dto = await ravendbsession.LoadAsync<Beverage>(aId.GetString());

            return dto.SetLikes(await documentCounters.GetAsync(Beverage.LikeCounterName))
                      .SetDislikes(await documentCounters.GetAsync(Beverage.DislikeCounterName));
        }

        public async Task<long> LikeBeverageAsync(Guid aId)
        {
            using IAsyncDocumentSession ravendbsession = ravenDB.OpenAsyncSession();

            IAsyncSessionDocumentCounters documentCounters = ravendbsession.CountersFor(aId.GetString());

            documentCounters.Increment(Beverage.LikeCounterName);

            await ravendbsession.SaveChangesAsync();

            return await documentCounters.GetAsync(Beverage.LikeCounterName) ?? 0;
        }

        public async Task<long> DislikeBeverageAsync(Guid aId)
        {
            using IAsyncDocumentSession ravendbsession = ravenDB.OpenAsyncSession();

            IAsyncSessionDocumentCounters documentCounters = ravendbsession.CountersFor(aId.GetString());

            documentCounters.Increment(Beverage.DislikeCounterName);

            await ravendbsession.SaveChangesAsync();

            return await documentCounters.GetAsync(Beverage.DislikeCounterName) ?? 0;
        }

        public async Task<IList<DTOMenuSearchResponse>> SearchAsync(string aSearch = "", int currentPage = 0)
        {
            using IAsyncDocumentSession ravendbsession = ravenDB.OpenAsyncSession();

            IRavenQueryable<Beverage> query;

            if (aSearch.IsNothing())
                query = ravendbsession.Query<Beverage>();
            else
            {
                aSearch = $"{aSearch.Trim()}*";
                query = ravendbsession.Query<Beverage>()
                                      .Search(x => x.Label, aSearch, 5)
                                      .Search(x => x.BeverageType, aSearch, 4)
                                      .Search(x => x.Country, aSearch, 3)
                                      .Search(x => x.Type, aSearch, 2)
                                      .Search(x => x.Variety, aSearch)
                                      .Search(x => x.Producer, aSearch)
                                      .Search(x => x.Vintage, aSearch)
                                      .Where(x => x.IsEnabled && !x.IsInvalid);
            }

            IList<Beverage> beverages = await query.Where(x => x.IsEnabled)
                                           .Skip(currentPage * 100)
                                           .Take(100)
                                           .ToListAsync();

            return [.. beverages.Select(Beverage => (DTOMenuSearchResponse)Beverage)];
        }

        public async Task<IList<DTOMenuSearchResponse>> VectorSearchAsync(string aSearch, int currentPage = 0)
        {
            using IAsyncDocumentSession ravendbsession = ravenDB.OpenAsyncSession();

            IList<Beverage> beverages = await ravendbsession.Advanced.AsyncDocumentQuery<Beverage>()
                                                                     .Skip(currentPage * 100)
                                                                     .Take(100)
                                                                     .VectorSearch(field => field.WithText(x => x.Description),
                                                                                   searchTerm => searchTerm.ByText(aSearch))
                                                                     .WaitForNonStaleResults()
                                                                     .ToListAsync();

            return [.. beverages.Select(Beverage => (DTOMenuSearchResponse)Beverage)];
        }

        public async Task<DTOOperationStatisticsResponse> CreateDataBulkInsertAsync()
        {
            DTOOperationStatisticsResponse dtoResponse = new("Creating mocked data into RavenDB with Bulk Insert");

            IList<Beverage> Beverages = Beverage.GetSamples(out DTOOperation dtoFileOperation);
            DTOOperation dtoBulkOperation = await BulkInsertAsync(Beverages);

            dtoResponse.DtoOperations.Add(dtoFileOperation);
            dtoResponse.DtoOperations.Add(dtoBulkOperation);

            return dtoResponse.Finish();
        }

        private async Task<DTOOperation> BulkInsertAsync(IList<Beverage> Beverages)
        {
            DTOOperation dtoBulkOperation = new($"Bulk Insert Operation on {Beverages.Count} documents.");

            using var bulkInsert = ravenDB.BulkInsert();

            foreach (Beverage Beverage in Beverages)
            {
                if (Beverage.IsValid(out _))
                    Beverage.Enable().NewId();

                await bulkInsert.StoreAsync(Beverage, Beverage.Id);
            }

            return dtoBulkOperation.Finish();
        }
    }
}
