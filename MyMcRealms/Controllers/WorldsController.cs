using Microsoft.AspNetCore.Mvc;
using MyMcRealms.Attributes;
using MyMcRealms.Entities;
using MyMcRealms.MyMcAPI.Responses;
using MyMcRealms.Responses;
using Semver;

namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class WorldsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ServersResponse>> GetWorlds()
        {
            string cookie = Request.Headers.Cookie;

            string playerUUID = cookie.Split(";")[0].Split(":")[2];
            string playerName = cookie.Split(";")[1].Split("=")[1];
            string gameVerision = cookie.Split(";")[2].Split("=")[1];

            List<WorldResponse> allWorlds = [];

            AllServersResponse AllServers = await new MyMcAPI.MyMcAPI(Environment.GetEnvironmentVariable("MYMC_API_KEY")).GetAllServers();

            foreach (var world in AllServers.Servers)
            {
                if (world.Online)
                {
                    if (world.WhitelistEnable && !(world.Whitelist.Any(p => p.Uuid.Replace("-", "") == playerUUID) || world.Ops.Any(p => p.Uuid.Replace("-", "") == playerUUID)))
                    {
                        continue;
                    }

                    int versionsCompared = SemVersion.Parse(gameVerision, SemVersionStyles.OptionalPatch).ComparePrecedenceTo(SemVersion.Parse(world.GameVersion, SemVersionStyles.OptionalPatch));
                    string isCompatible = versionsCompared == 0 ? "COMPATIBLE" : versionsCompared < 0 ? "NEEDS_DOWNGRADE" : "NEEDS_UPGRADE";

                    bool isOlderVersion = SemVersion.Parse(gameVerision, SemVersionStyles.OptionalPatch).ComparePrecedenceTo(SemVersion.Parse("1.20.3", SemVersionStyles.OptionalPatch)) < 0;
                    string isCompatibleOnOlderVersions = (isOlderVersion && isCompatible.StartsWith("NEEDS_")) ? "CLOSED" : "OPEN";

                    string worldOwnerName = world.Ops.ToArray().Length == 0 ? "Owner" : world.Ops[0].Name;
                    string worldOwnerUuid = world.Ops.ToArray().Length == 0 ? "069a79f444e94726a5befca90e38aaf5" : world.Ops[0].Uuid;
                    string worldName = world.Ops.ToArray().Length == 0 ? world.ServerName : $"{world.Ops[0].Name}'s server";

                    WorldResponse response = new()
                    {
                        Id = AllServers.Servers.IndexOf(world),
                        Owner = worldOwnerName,
                        OwnerUUID = worldOwnerUuid,
                        Name = worldName,
                        Motd = world.Motd,
                        State = isCompatibleOnOlderVersions,
                        WorldType = "NORMAL",
                        MaxPlayers = 10,
                        MinigameId = null,
                        MinigameName = null,
                        MinigameImage = null,
                        ActiveSlot = 1,
                        Member = false,
                        DaysLeft = 7,
                        Expired = false,
                        ExpiredTrial = false,
                        Compatibility = isCompatible,
                        ActiveVersion = world.GameVersion
                    };

                    allWorlds.Add(response);
                }
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
            var worlds = await new MyMcAPI.MyMcAPI(Environment.GetEnvironmentVariable("MYMC_API_KEY")).GetAllServers();
            var world = worlds.Servers[id];

            string worldOwnerName = world.Ops.ToArray().Length == 0 ? "Owner" : world.Ops[0].Name;
            string worldOwnerUuid = world.Ops.ToArray().Length == 0 ? "069a79f444e94726a5befca90e38aaf5" : world.Ops[0].Uuid;
            string worldName = world.Ops.ToArray().Length == 0 ? world.ServerName : $"{world.Ops[0].Name}'s server";
            List<Player> whitelistedPlayers = [];

            foreach (var player in world.Whitelist)
            {
                Player whitelistedPlayer = new()
                {
                    Name = player.Name,
                    Uuid = player.Uuid,
                    Accepted = true,
                    Online = false,
                    Operator = world.Ops.Find(p => p.Name == player.Name) != null,
                    Permission = world.Ops.Find(p => p.Name == player.Name) != null ? "OPERATOR" : "MEMBER",
                };

                whitelistedPlayers.Add(whitelistedPlayer);
            }

            WorldResponse response = new()
            {
                Id = id,
                Owner = worldOwnerName,
                OwnerUUID = worldOwnerUuid,
                Name = worldName,
                Motd = world.Motd,
                State = world.WhitelistEnable ? "CLOSED" : "OPEN",
                WorldType = "NORMAL",
                MaxPlayers = 10,
                MinigameId = null,
                MinigameName = null,
                MinigameImage = null,
                ActiveSlot = 1,
                Member = false,
                Players = whitelistedPlayers,
                DaysLeft = 7,
                Expired = false,
                ExpiredTrial = false,
                ActiveVersion = world.GameVersion
            };

            return Ok(response);
        }

        [HttpPut("{id}/open")]
        public async Task<ActionResult<bool>> Open(int id)
        {
            var _api = new MyMcAPI.MyMcAPI(Environment.GetEnvironmentVariable("MYMC_API_KEY"));

            var world = (await _api.GetAllServers()).Servers[id];
            var api = new MyMcAPI.MyMcAPI(world.OwnersToken);

            if (world == null) return NotFound("World not found");

            api.ExecuteCommand("whitelist off");

            return Ok(true);
        }

        [HttpPut("{id}/close")]
        public async Task<ActionResult<bool>> Close(int id)
        {
            var _api = new MyMcAPI.MyMcAPI(Environment.GetEnvironmentVariable("MYMC_API_KEY"));

            var world = (await _api.GetAllServers()).Servers[id];
            var api = new MyMcAPI.MyMcAPI(world.OwnersToken);

            if (world == null) return NotFound("World not found");

            api.ExecuteCommand("whitelist on");

            return Ok(true);
        }

        [HttpGet("v1/{wId}/join/pc")]
        public async Task<ActionResult<ConnectionResponse>> Join(int wId)
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
