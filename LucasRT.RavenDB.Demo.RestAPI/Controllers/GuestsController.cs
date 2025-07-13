using LeadSoft.Common.Library.Constants;
using LucasRT.RavenDB.Demo.Application.Interfaces.Guests;
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
        /// Handles an HTTP GET request and returns a successful response.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing a 200 OK response.</returns>
        [HttpPost("", Name = nameof(PostAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<IActionResult> PostAsync()
        {
            return Ok(await guestService.GetAIContext());
        }

        [HttpPatch("", Name = nameof(PatchAsync))]
        [Produces(Constant.ApplicationJson)]
        public async Task<IActionResult> PatchAsync([FromQuery] Guid aChatId, [FromBody] string aMessage)
        {
            return Ok(await guestService.FeedAIContext(aChatId, aMessage));
        }
    }
}
