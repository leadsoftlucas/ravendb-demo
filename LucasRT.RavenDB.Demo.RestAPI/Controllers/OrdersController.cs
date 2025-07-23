using LeadSoft.Common.Library.Constants;
using LucasRT.RavenDB.Demo.Application.Interfaces.Menus;
using LucasRT.RavenDB.Demo.Domain.DTOs.Orders;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static LeadSoft.Common.Library.Enumerators.Enums;

namespace LucasRT.RavenDB.Demo.Controllers
{
    /// <summary>
    /// This controller provides API endpoints for managing orders in the RavenDB database.
    /// </summary>
    /// <param name="ordersBuilder"></param>
    /// <param name="ordersService"></param>
    /// <param name="logger"></param>
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Menus Controller | Use this to manage orders in the RavenDB database.")]
    public class OrdersController(IOrdersSingleton ordersBuilder, IOrdersService ordersService, ILogger<OrdersController> logger) : ControllerBase
    {
        /// <summary>
        /// Gets a specific Order by its ID.
        /// </summary>
        /// <param name="aId">Order Id</param>
        /// <returns></returns>
        [HttpHead]
        [HttpGet("{aId:guid}", Name = nameof(GetOrderAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOOrderResponse>> GetOrderAsync([FromRoute] Guid aId)
            => Ok(await ordersService.LoadAsync(aId));

        /// <summary>
        /// Gets Order annual dashboard data.
        /// </summary>        
        [HttpGet("Dashboard/Annual", Name = nameof(GetAnnualDashboardAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<IList<DTOOrderAnnualDashboardResponse>>> GetAnnualDashboardAsync([FromQuery] Guid? aGuestId = null, [FromQuery] Guid? aBeverageId = null)
            => Ok(await ordersService.AnnualDashboardAsync(aGuestId, aBeverageId));

        [HttpGet("Dashboard/{aYear:int}/{aMonth}", Name = nameof(GetMonthlyDashboardAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOOrderMonthlyDashboardResponse>> GetMonthlyDashboardAsync([FromRoute] int aYear, [FromRoute] Month aMonth, [FromQuery] Guid? aGuestId = null, [FromQuery] Guid? aBeverageId = null)
            => Ok(await ordersService.MonthlyDashboardAsync(aYear, aMonth, aGuestId, aBeverageId));

        [HttpPost("Populate/Start", Name = nameof(StartAttendanceAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<IActionResult> StartAttendanceAsync()
        {
            await ordersBuilder.StartAttendanceAsync();
            return Ok();
        }

        [HttpPost("Populate/Stop", Name = nameof(StopAttendanceAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<IActionResult> StopAttendanceAsync()
        {
            await ordersBuilder.StopAttendanceAsync();
            return Ok();
        }
    }
}
