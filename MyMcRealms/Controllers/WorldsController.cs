using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMcRealms.Attributes;
using MyMcRealms.Data;
using MyMcRealms.Entities;
using MyMcRealms.MyMcAPI;
using MyMcRealms.MyMcAPI.Responses;
using MyMcRealms.Requests;
using MyMcRealms.Responses;
using Newtonsoft.Json;

namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class WorldsController : ControllerBase
    {
        private readonly DataContext _context;

        public WorldsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ServersResponse>> GetWorlds()
        {
            string cookie = Request.Headers.Cookie;

            string playerUUID = cookie.Split(";")[0].Split(":")[2];
            string playerName = cookie.Split(";")[1].Split("=")[1];

            List<WorldResponse> allWorlds = [];

            AllServersResponse AllServers = await new MyMcAPI.MyMcAPI(Environment.GetEnvironmentVariable("MYMC_API_KEY")).GetAllServers();

            foreach (var world in AllServers.Servers)
            {
                WorldResponse response = new()
                {
                    Id = AllServers.Servers.IndexOf(world),
                    Owner = "Owner",
                    OwnerUUID = "87a2931cf37f4867b6dd1f0699e138d3s",
                    Name = "my-mc.link world",
                    Motd = "A world hosted on my-mc.link",
                    State = "OPEN",
                    WorldType = "NORMAL",
                    MaxPlayers = 10,
                    MinigameId = null,
                    MinigameName = null,
                    MinigameImage = null,
                    ActiveSlot = 1,
                    Member = false,
                    Players = [],
                    DaysLeft = 0,
                    Expired = false,
                    ExpiredTrial = false
                };

                allWorlds.Add(response);
            }

            ServersResponse servers = new()
            {
                Servers = allWorlds
            };

            return Ok(servers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorldResponse>> GetWorldById(int id)
        {
            var world = await _context.Worlds.Include(w => w.Players).Include(w => w.Subscription).FirstOrDefaultAsync(w => w.Id == id);

            if (world?.Subscription == null) return NotFound("World not found");

            WorldResponse response = new()
            {
                Id = world.Id,
                Owner = world.Owner,
                OwnerUUID = world.OwnerUUID,
                Name = world.Name,
                Motd = world.Motd,
                State = world.State,
                WorldType = world.WorldType,
                MaxPlayers = world.MaxPlayers,
                MinigameId = world.MinigameId,
                MinigameName = world.MinigameName,
                MinigameImage = world.MinigameImage,
                ActiveSlot = world.ActiveSlot,
                Member = world.Member,
                Players = world.Players,
                DaysLeft = ((DateTimeOffset)world.Subscription.StartDate.AddDays(30) - DateTime.Today).Days,
                Expired = ((DateTimeOffset)world.Subscription.StartDate.AddDays(30) - DateTime.Today).Days < 0,
                ExpiredTrial = false
            };

            return response;
        }

        [HttpPost("{id}/initialize")]
        public async Task<ActionResult<World>> Initialize(int id, WorldCreateRequest body)
        {
            var worlds = await _context.Worlds.ToListAsync();

            var world = worlds.Find(w => w.Id == id);

            if (world == null) return NotFound("World not found");
            if (world.State != "UNINITIALIZED") return NotFound("World already initialized");

            var subscription = new Subscription
            {
                StartDate = DateTime.UtcNow,
                SubscriptionType = "NORMAL"
            };

            world.Name = body.Name;
            world.Motd = body.Description;
            world.State = "OPEN";
            world.Subscription = subscription;

            var defaultServerAddress = _context.Configuration.FirstOrDefault(x => x.Key == "defaultServerAddress");

            var connection = new Connection
            {
                World = world,
                Address = JsonConvert.DeserializeObject(defaultServerAddress.Value)
            };

            _context.Worlds.Update(world);

            _context.Subscriptions.Add(subscription);
            _context.Connections.Add(connection);

            _context.SaveChanges();

            return Ok(world);
        }

        [HttpPost("{id}/reset")]
        public ActionResult<bool> Reset(int id)
        {
            Console.WriteLine($"Resetting world {id}");
            return Ok(true);
        }

        [HttpPut("{id}/open")]
        public async Task<ActionResult<bool>> Open(int id)
        {
            var worlds = await _context.Worlds.ToListAsync();

            var world = worlds.Find(w => w.Id == id);

            if (world == null) return NotFound("World not found");

            world.State = "OPEN";

            _context.SaveChanges();

            return Ok(true);
        }

        [HttpPut("{id}/close")]
        public async Task<ActionResult<bool>> Close(int id)
        {
            var worlds = await _context.Worlds.ToListAsync();

            var world = worlds.FirstOrDefault(w => w.Id == id);

            if (world == null) return NotFound("World not found");

            world.State = "CLOSED";

            _context.SaveChanges();

            return Ok(true);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<bool>> UpdateWorld(int id, WorldCreateRequest body)
        {
            var worlds = await _context.Worlds.ToListAsync();

            var world = worlds.Find(w => w.Id == id);

            if (world == null) return NotFound("World not found");

            world.Name = body.Name;
            world.Motd = body.Description;

            _context.SaveChanges();

            return Ok(true);
        }

        [HttpPost("{wId}/slot/{sId}")]
        public bool U(int wId, int sId, object o)
        {
            Console.WriteLine(o);
            return true;
        }

        [HttpGet("{Id}/backups")]
        public async Task<ActionResult<BackupsResponse>> GetBackups(int id)
        {
            var backups = await _context.Backups.Where(b => b.World.Id == id).ToListAsync();

            BackupsResponse worldBackups = new()
            {
                Backups = backups
            };

            return Ok(worldBackups);
        }

        [HttpGet("v1/{wId}/join/pc")]
        public async Task<ActionResult<Connection>> Join(int wId)
        {
            AllServersResponse AllServers = await new MyMcAPI.MyMcAPI(Environment.GetEnvironmentVariable("MYMC_API_KEY")).GetAllServers();

            ConnectionResponse connection = new()
            {
                Address = AllServers.Servers[wId].Connect,
                PendingUpdate = false
            };

            return Ok(connection);
        }
    }
}
