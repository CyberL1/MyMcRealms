using Microsoft.AspNetCore.Mvc;
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
            return BadRequest("No subscription for you :(");
        }
    }
}