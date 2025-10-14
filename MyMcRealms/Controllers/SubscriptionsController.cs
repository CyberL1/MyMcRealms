using Microsoft.AspNetCore.Mvc;
using Minecraft_Realms_Emulator.Responses;
using MyMcRealms.Attributes;
using MyMcRealms.Responses;

namespace MyMcRealms.Controllers
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
            var sub = new SubscriptionResponse
            {
                StartDate = ((DateTimeOffset)DateTime.Today).ToUnixTimeMilliseconds(),
                DaysLeft =  ((DateTimeOffset)DateTime.Today.AddDays(30) - DateTime.Today).Days,
                SubscriptionType = "NORMAL"
            };

            return Ok(sub);
        }
    }
}