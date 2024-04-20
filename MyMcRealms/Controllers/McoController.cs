using Microsoft.AspNetCore.Mvc;
using MyMcRealms.Attributes;
using MyMcRealms.Data;
using MyMcRealms.Responses;
using Newtonsoft.Json;

namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class McoController : ControllerBase
    {
        private readonly DataContext _context;

        public McoController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("available")]
        public bool GetAvailable()
        {
            return true;
        }

        [HttpGet("client/compatible")]
        public string GetCompatible()
        {
            return Compatility.COMPATIBLE.ToString();
        }

        [HttpGet("v1/news")]
        public NewsResponse GetNews()
        {
            var newsLink = _context.Configuration.FirstOrDefault(s => s.Key == "newsLink");

            var news = new NewsResponse
            {
                NewsLink = JsonConvert.DeserializeObject(newsLink.Value),
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