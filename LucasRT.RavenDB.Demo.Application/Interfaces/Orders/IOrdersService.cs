namespace LucasRT.RavenDB.Demo.Application.Interfaces.Menus
{
    public interface IOrdersService
    {
        Task<object> LoadAsync(Guid aId);
        Task<IList<object>> VectorSearchAsync(string aSearch, int currentPage = 0);
    }
}
