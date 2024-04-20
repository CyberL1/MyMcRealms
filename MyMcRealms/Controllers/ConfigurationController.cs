using Microsoft.AspNetCore.Mvc;
using MyMcRealms.Data;
using MyMcRealms.Entities;

namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly DataContext _context;

        public ConfigurationController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<Configuration> GetConfigurationAsync()
        {
            var configuration = _context.Configuration;
            return Ok(configuration);
        }
    }
}