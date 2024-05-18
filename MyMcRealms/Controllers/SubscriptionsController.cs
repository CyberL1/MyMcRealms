using Microsoft.AspNetCore.Mvc;
using MyMcRealms.Attributes;

namespace Minecraft_Realms_Emulator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class SubscriptionsController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<string> GetSubscription(int id)
        {
            return BadRequest("No subscription for you :(");
        }
    }
}