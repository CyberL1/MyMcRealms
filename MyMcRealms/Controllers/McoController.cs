using Microsoft.AspNetCore.Mvc;
using MyMcRealms.Attributes;
using MyMcRealms.Responses;

namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class McoController : ControllerBase
    {
        [HttpGet("available")]
        public bool GetAvailable()
        {
            return true;
        }

        [HttpGet("client/compatible")]
        public string GetCompatible()
        {
            return "COMPATIBLE";
        }

        [HttpGet("v1/news")]
        public NewsResponse GetNews()
        {
            var news = new NewsResponse
            {
                NewsLink = "https://github.com/CyberL1/MyMcRealms",
            };

            return news;
        }

        [HttpPost("tos/agreed")]
        public ActionResult TosAgree()
        {
            return Ok();
        }
    }
}