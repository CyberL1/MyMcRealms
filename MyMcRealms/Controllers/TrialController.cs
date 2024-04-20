using Microsoft.AspNetCore.Mvc;
using MyMcRealms.Attributes;
using MyMcRealms.Data;

namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class TrialController : ControllerBase
    {
        private readonly DataContext _context;

        public TrialController(DataContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetTrial")]
        public bool Get() {
            return bool.Parse(_context.Configuration.FirstOrDefault(x => x.Key == "trialMode").Value);
        }
    }
}
