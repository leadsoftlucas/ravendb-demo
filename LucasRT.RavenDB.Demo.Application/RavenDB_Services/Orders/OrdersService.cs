using LeadSoft.Adapter.OpenAI_Bridge;
using LucasRT.RavenDB.Demo.Application.Interfaces.Menus;
using Raven.Client.Documents;

namespace LucasRT.RavenDB.Demo.Application.RavenDB_Services.Orders
{
    /// <summary>
    /// Demo - up to 10 minutes
    /// Querying & indexing in ravendb - intent to blow our mind
    /// focus on speed of development and the performance at runtime
    /// overall complexity of the solution - contrast with other alternatives and show how we do better
    /// </summary>
    public class OrdersService(IDocumentStore ravenDB, IOpen_AI openAI) : IOrdersService
    {
        public Task<object> LoadAsync(Guid aId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<object>> VectorSearchAsync(string aSearch, int currentPage = 0)
        {
            throw new NotImplementedException();
        }
    }
}
