using LucasRT.RavenDB.Demo.Domain.DTOs.Orders;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace LucasRT.RavenDB.Demo.Application.Interfaces.Menus
{
    public interface IOrdersService
    {
        Task<DTOOrderResponse> LoadAsync(Guid aId);
        Task<IList<DTOOrderAnnualDashboardResponse>> AnnualDashboardAsync(Guid? aGuestId = null, Guid? aBeverageId = null);
        Task<DTOOrderMonthlyDashboardResponse> MonthlyDashboardAsync(int aYear, Month aMonth, Guid? aGuestId = null, Guid? aBeverageId = null);
    }
}
