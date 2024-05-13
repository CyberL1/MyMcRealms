using Microsoft.AspNetCore.Mvc;
using MyMcRealms.Attributes;

namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class TrialController : ControllerBase
    {
        [HttpGet(Name = "GetTrial")]
        public bool Get() {
            return false;
        }
    }
}
