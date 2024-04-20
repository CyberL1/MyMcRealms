using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMcRealms.Attributes;
using MyMcRealms.Data;
using MyMcRealms.Responses;

namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class SubscriptionsController : ControllerBase
    {
        private readonly DataContext _context;

        public SubscriptionsController(DataContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionResponse>> Get(int id)
        {
            var world = await _context.Worlds.Include(w => w.Subscription).FirstOrDefaultAsync(w => w.Id == id);

            if (world?.Subscription == null) return NotFound("Subscription not found");

            var sub = new SubscriptionResponse
            {
                StartDate = ((DateTimeOffset)world.Subscription.StartDate).ToUnixTimeMilliseconds(),
                DaysLeft =  ((DateTimeOffset)world.Subscription.StartDate.AddDays(30) - DateTime.Today).Days,
                SubscriptionType = world.Subscription.SubscriptionType
            };

            return Ok(sub);
        }
    }
}
