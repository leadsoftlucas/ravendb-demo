using LucasRT.RavenDB.Demo.Domain.DTOs;
using LucasRT.RavenDB.Demo.Domain.DTOs.Menus;

namespace LucasRT.RavenDB.Demo.Application.Interfaces.Menus
{
    public interface IMenusService
    {
        Task<DTOMenuResponse> LoadAsync(Guid aId);
        Task<long> LikeBeverageAsync(Guid aId);
        Task<long> DislikeBeverageAsync(Guid aId);
        Task<IList<DTOMenuSearchResponse>> SearchAsync(string aSearch = "", int currentPage = 0);
        Task<IList<DTOMenuSearchResponse>> VectorSearchAsync(string aSearch, int currentPage = 0);
        Task<DTOOperationStatisticsResponse> CreateDataBulkInsertAsync();
    }
}
