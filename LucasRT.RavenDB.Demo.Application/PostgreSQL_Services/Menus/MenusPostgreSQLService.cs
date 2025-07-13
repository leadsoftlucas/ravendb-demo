using EFCore.BulkExtensions;
using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.Demo.Application.Interfaces.Menus;
using LucasRT.RavenDB.Demo.Application.PostgreSQL_Services.Data;
using LucasRT.RavenDB.Demo.Domain.DTOs;
using LucasRT.RavenDB.Demo.Domain.DTOs.Menus;
using LucasRT.RavenDB.Demo.Domain.Entities.Menus;
using Microsoft.EntityFrameworkCore;

namespace LucasRT.RavenDB.Demo.Application.PostgreSQL_Services.Menus
{
    public class MenusPostgreSQLService(IUnitOfWork unitOfWork) : IMenusService
    {
        public async Task<DTOMenuResponse> LoadAsync(Guid aId)
            => await unitOfWork.GetDbContext().Beverages.AsQueryable<Beverage>()
                                                        .Where(b => b.Id == aId.GetString())
                                                        .FirstOrDefaultAsync();

        public async Task<IList<DTOMenuSearchResponse>> SearchAsync(string aSearch = "", int currentPage = 0)
        {
            IQueryable<Beverage> query = unitOfWork.GetDbContext().Set<Beverage>()
                                                                  .Where(x => x.IsEnabled);

            if (!string.IsNullOrWhiteSpace(aSearch))
            {
                aSearch = aSearch.Trim();

                query = query.Where(x => EF.Functions.ILike(x.Label, $"%{aSearch}%") ||
                                         EF.Functions.ILike(x.BeverageType.ToString(), $"%{aSearch}%") ||
                                         EF.Functions.ILike(x.Country, $"%{aSearch}%") ||
                                         EF.Functions.ILike(x.Type, $"%{aSearch}%") ||
                                         EF.Functions.ILike(x.Variety ?? "", $"%{aSearch}%") ||
                                         EF.Functions.ILike(x.Producer ?? "", $"%{aSearch}%") ||
                                         EF.Functions.ILike(x.Vintage.ToString(), $"%{aSearch}%")
                );
            }

            List<Beverage> beverages = await query.Skip(currentPage * 100)
                                                  .Take(100)
                                                  .ToListAsync();

            return [.. beverages.Select(Beverage => (DTOMenuSearchResponse)Beverage)];
        }

        public async Task<IList<DTOMenuSearchResponse>> VectorSearchAsync(string aSearch, int currentPage = 0)
        {
            List<Beverage> beverages = await unitOfWork.GetDbContext().Beverages.Where(b => EF.Functions.ToTsVector("english", b.Description)
                                                                                                        .Matches(EF.Functions.PlainToTsQuery("english", aSearch)))
                                                                                .Skip(currentPage * 100)
                                                                                .Take(100)
                                                                                .ToListAsync();

            return [.. beverages.Select(Beverage => (DTOMenuSearchResponse)Beverage)];
        }

        public async Task<DTOOperationStatisticsResponse> CreateDataBulkInsertAsync()
        {
            DTOOperationStatisticsResponse dtoResponse = new("Creating mocked data into PostgreSQL with Bulk Insert.");

            IList<Beverage> Beverages = Beverage.GetSamples(out DTOOperation dtoFileOperation);
            DTOOperation dtoBulkOperation = await BulkInsertAsync(Beverages);

            dtoResponse.DtoOperations.Add(dtoFileOperation);
            dtoResponse.DtoOperations.Add(dtoBulkOperation);

            return dtoResponse.Finish();
        }

        private async Task<DTOOperation> BulkInsertAsync(IList<Beverage> Beverages)
        {
            DTOOperation dtoBulkOperation = new($"Bulk Insert Operation on {Beverages.Count} records.");

            foreach (var beverage in Beverages)
            {
                if (beverage.IsValid(out _))
                    beverage.Enable().NewId();
            }

            await unitOfWork.GetDbContext().BulkInsertAsync(Beverages.ToList());

            return dtoBulkOperation.Finish();
        }

        public Task<long> LikeBeverageAsync(Guid aId)
        {
            throw new NotImplementedException();
        }

        public Task<long> DislikeBeverageAsync(Guid aId)
        {
            throw new NotImplementedException();
        }
    }
}
