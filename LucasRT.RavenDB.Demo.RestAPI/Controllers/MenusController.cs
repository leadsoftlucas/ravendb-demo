using LeadSoft.Common.Library.Constants;
using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.Demo.Application.Interfaces.Menus;
using LucasRT.RavenDB.Demo.Domain.DTOs;
using LucasRT.RavenDB.Demo.Domain.DTOs.Menus;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LucasRT.RavenDB.Demo.Controllers
{
    /// <summary>
    /// This controller provides API endpoints for managing menu items in the RavenDB database.
    /// </summary>
    /// <param name="menusService"></param>
    /// <param name="logger"></param>
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Menus Controller | Use this to manage menu items in the RavenDB database.")]
    public class MenusController(IMenusService menusService, ILogger<MenusController> logger) : ControllerBase
    {
        /// <summary>
        /// Get a specific menu item by its ID from the database.
        /// </summary>
        /// <param name="id">Beverage Id</param>
        [HttpHead]
        [HttpGet("{id:guid}", Name = nameof(GetMenuAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOMenuResponse>> GetMenuAsync([FromRoute] Guid id)
        {
            DTOMenuResponse dto = await menusService.LoadAsync(id);

            return dto.IsNotNull() ? Ok(dto) : NotFound();
        }

        /// <summary>
        /// Search for Beverages in the RavenDB database.
        /// </summary>
        /// <remarks>
        /// Page limited to 100 items per page.
        /// If no search text is provided, all Beverages will be listed.
        /// </remarks>
        /// <param name="searchtext">Search text to lookup database. Black to list all.</param>
        /// <param name="currentPage">Current page. Default for the first page. Page limited to 100.</param>
        [HttpGet("", Name = nameof(GetMenuSearchAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<IList<DTOMenuSearchResponse>>> GetMenuSearchAsync([FromQuery] string searchtext = "", [FromQuery] int currentPage = 0)
            => Ok(await menusService.SearchAsync(searchtext, Math.Abs(currentPage)));

        /// <summary>
        /// Vector search for Beverages in the database.
        /// </summary>
        /// <remarks>
        /// Page limited to 100 items per page.
        /// Search text is mandatory and you can ask what you need that the vector search will interpretate your question and the database content.
        /// </remarks>
        /// <param name="searchtext"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpGet("Vector/", Name = nameof(GetMenuVectorSearchAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<IList<DTOMenuSearchResponse>>> GetMenuVectorSearchAsync([FromQuery] string searchtext, [FromQuery] int currentPage = 0)
        {
            if (searchtext.IsNothing())
                return BadRequest("Search text cannot be empty for vector search.");

            return Ok(await menusService.VectorSearchAsync(searchtext, Math.Abs(currentPage)));
        }

        /// <summary>
        /// Method available to increment the like counter of a specific Beverage.
        /// </summary>
        [HttpPost("Beverages/Like/{id:guid}", Name = nameof(PostLikeBeverageAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<long>> PostLikeBeverageAsync([FromRoute] Guid id)
         => Ok(await menusService.LikeBeverageAsync(id));

        /// <summary>
        /// Method available to increment the dislike counter of a specific Beverage.
        /// </summary>
        [HttpPost("Beverages/Dislike/{id:guid}", Name = nameof(PostDislikeBeverageAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<long>> PostDislikeBeverageAsync([FromRoute] Guid id)
         => Ok(await menusService.DislikeBeverageAsync(id));

        /// <summary>
        /// Bulk insert sample data into the database with 30.000 Beverages, as wines, whikies and beers at once!
        /// </summary>
        /// <remarks>
        /// Multiple calls might result in duplicate data.
        /// </remarks>
        [HttpPost("Samples/BulkInsert", Name = nameof(PostMenuSamplesBulkInsertAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOOperationStatisticsResponse>> PostMenuSamplesBulkInsertAsync()
         => Ok(await menusService.CreateDataBulkInsertAsync());
    }
}
