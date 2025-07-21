using LeadSoft.Adapter.OpenAI_Bridge.DTOs;
using LeadSoft.Common.Library.Constants;
using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.Demo.Application.Interfaces.Guests;
using LucasRT.RavenDB.Demo.Domain.DTOs;
using LucasRT.RavenDB.Demo.Domain.DTOs.Guests;
using LucasRT.RavenDB.Demo.Domain.DTOs.Menus;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LucasRT.RavenDB.Demo.Controllers
{
    /// <summary>
    /// Provides API endpoints for guest-related operations.
    /// </summary>
    /// <remarks>This controller is intended for use by guests interacting with the application. It includes
    /// endpoints for retrieving guest-specific data and performing guest-related actions.</remarks>
    /// <param name="logger"></param>
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Guests Controller | Use this to use this app as a guest.")]
    public class GuestsController(IGuestsService guestService, ILogger<GuestsController> logger) : ControllerBase
    {
        /// <summary>
        /// Method to retrieve a specific guest by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpHead]
        [HttpGet("{id:guid}", Name = nameof(GetGuestAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOGuestResponse>> GetGuestAsync([FromRoute] Guid id)
        {
            DTOGuestResponse dto = await guestService.LoadAsync(id);

            return dto.IsNotNull() ? Ok(dto) : NotFound();
        }

        /// <summary>
        /// Method to search text guests
        /// </summary>
        /// <param name="searchtext">Search text to lookup database. Black to list all.</param>
        /// <param name="currentPage">Current page. Default for the first page. Page limited to 100.</param>
        [HttpGet("", Name = nameof(GetGuestsAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOGuestResponse>> GetGuestsAsync([FromQuery] string searchtext = "", [FromQuery] int currentPage = 0)
            => Ok(await guestService.SearchAsync(searchtext, Math.Abs(currentPage)));

        /// <summary>
        /// Vector search for Guests base knowledge in the database.
        /// </summary>
        /// <remarks>
        /// Page limited to 100 items per page.
        /// Search text is mandatory and you can ask what you need that the vector search will interpretate your question and the database content.
        /// </remarks>
        /// <param name="searchtext"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpGet("Vector/", Name = nameof(GetGuestsVectorSearchAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<IList<DTOMenuSearchResponse>>> GetGuestsVectorSearchAsync([FromQuery] string searchtext, [FromQuery] int currentPage = 0)
        {
            if (searchtext.IsNothing())
                return BadRequest("Search text cannot be empty for vector search.");

            return Ok(await guestService.VectorSearchAsync(searchtext, Math.Abs(currentPage)));
        }

        /// <summary>
        /// Method to post a regular form Guest registration
        /// </summary>
        /// <returns></returns>
        [HttpPost("", Name = nameof(PostGuestAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOGuestResponse>> PostGuestAsync([FromBody] DTOGuestRequest dto)
            => Ok(await guestService.CreateAsync(dto));

        /// <summary>
        /// Method to retrieve the AI context for guest welcome registration.
        /// </summary>
        /// <returns></returns>
        [HttpPost("Chat", Name = nameof(PostGuestWelcomeAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOChatHistory>> PostGuestWelcomeAsync()
            => Ok(await guestService.GetAIContext());

        /// <summary>
        /// Method to patch the guest response with a message.
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="aMessage"></param>
        /// <returns></returns>
        [HttpPatch("Chat/{chatId:guid}", Name = nameof(PatchGuestResponseAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOChatHistory>> PatchGuestResponseAsync([FromRoute] Guid chatId, [FromBody] string aMessage)
            => Ok(await guestService.FeedAIContext(chatId, aMessage));

        /// <summary>
        /// Bulk insert sample data into the database with 50.000 Guests at once!
        /// </summary>
        /// <remarks>
        /// Multiple calls might result in duplicate data.
        /// </remarks>
        [HttpPost("Samples/BulkInsert", Name = nameof(PostGuestSamplesBulkInsertAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOOperationStatisticsResponse>> PostGuestSamplesBulkInsertAsync()
         => Ok(await guestService.CreateDataBulkInsertAsync());
    }
}
