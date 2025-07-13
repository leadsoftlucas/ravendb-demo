using LeadSoft.Adapter.OpenAI_Bridge.DTOs;
using LeadSoft.Common.Library.Constants;
using LeadSoft.Common.Library.Extensions;
using LucasRT.RavenDB.Demo.Application.Interfaces.Guests;
using LucasRT.RavenDB.Demo.Domain.DTOs.Guests;
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
        /// Method to retrieve the AI context for a guest.
        /// </summary>
        /// <returns></returns>
        [HttpPost("", Name = nameof(PostGuestWelcomeAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOChatHistory>> PostGuestWelcomeAsync()
            => Ok(await guestService.GetAIContext());

        /// <summary>
        /// Method to patch the guest response with a message.
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="aMessage"></param>
        /// <returns></returns>
        [HttpPatch("{chatId:guid}", Name = nameof(PatchGuestResponseAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<ActionResult<DTOChatHistory>> PatchGuestResponseAsync([FromRoute] Guid chatId, [FromBody] string aMessage)
            => Ok(await guestService.FeedAIContext(chatId, aMessage));
    }
}
