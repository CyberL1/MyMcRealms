using Microsoft.AspNetCore.Mvc;
using MyMcRealms.Attributes;

namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class TrialController : ControllerBase
    {
        [HttpGet]
        public bool GetTrial() {
            return false;
        }
    }
}
