using Microsoft.AspNetCore.Mvc;
using Minecraft_Realms_Emulator.Responses;
using MyMcRealms.Attributes;

namespace Minecraft_Realms_Emulator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class SubscriptionsController : ControllerBase
    {
        [HttpGet("{wId}")]
        [CheckRealmOwner]
        public ActionResult<string> GetSubscription(int wId)
        {
            ErrorResponse errorResponse = new()
            {
                ErrorCode = 400,
                ErrorMsg = "No subscription for you :("
            };

            return BadRequest(errorResponse);
        }
    }
}