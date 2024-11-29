using Microsoft.AspNetCore.Mvc;
using Minecraft_Realms_Emulator.Responses;
using MyMcRealms.Attributes;
using MyMcRealms.MyMcAPI.Responses;
using MyMcRealms.Responses;
using Newtonsoft.Json;
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

            AllServersResponse AllServers = await new MyMcAPI.Wrapper(Environment.GetEnvironmentVariable("MYMC_API_KEY")).GetAllServers();

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

                    bool isCompatibleOnOlderVersions = isOlderVersion && isCompatible == "COMPATIBLE";
                    bool isBanned = world.Banlist.Any(p => p.Name == playerName);

                    string worldOwnerName = world.Ops.ToArray().Length == 0 ? "Not claimed" : world.Ops[0].Name;
                    string worldOwnerUuid = world.Ops.ToArray().Length == 0 ? "069a79f444e94726a5befca90e38aaf5" : world.Ops[0].Uuid.Replace("-", "");
                    string worldName = world.Ops.ToArray().Length == 0 ? world.ServerName : $"{world.Ops[0].Name}'s server";
                    string worldState = !isBanned ? "OPEN" : "CLOSED";

                    if (isOlderVersion && !isCompatibleOnOlderVersions)
                    {
                        worldState = "CLOSED";
                    }

                    if (SemVersion.Parse(gameVerision, SemVersionStyles.OptionalPatch).ComparePrecedenceTo(SemVersion.Parse("1.19.4", SemVersionStyles.OptionalPatch)) < 0)
                    {
                        if (world.Motd.Length > 32)
                        {
                            world.Motd = world.Motd.Remove(32); // Pre 1.19.4 MOTD limit
                        }
                    }
                    else
                    {
                        if (world.Motd.Length > 52)
                        {
                            world.Motd = world.Motd.Remove(52); // Post 1.19.4 MOTD limit
                        }
                    }

                    WorldResponse response = new()
                    {
                        Id = AllServers.Servers.IndexOf(world),
                        Owner = worldOwnerName,
                        OwnerUUID = worldOwnerUuid,
                        Name = worldName,
                        Motd = world.Motd.Replace("�", "§").Replace("&", "§"),
                        State = worldState,
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

        [HttpGet("{wId}")]
        [CheckRealmOwner]
        public async Task<ActionResult<WorldResponse>> GetWorldById(int wId)
        {
            var _api = new MyMcAPI.Wrapper(Environment.GetEnvironmentVariable("MYMC_API_KEY"));

            var world = (await _api.GetAllServers()).Servers[wId];

            var api = new MyMcAPI.Wrapper(world.OwnersToken);
            var whitelist = await api.GetWhitelist();

            if (whitelist == null)
            {
                ErrorResponse errorResponse = new()
                {
                    ErrorCode = 400,
                    ErrorMsg = $"Cannot get data for world {wId}"
                };

                return BadRequest(errorResponse);
            }

            string worldOwnerName = world.Ops[0].Name;
            string worldOwnerUuid = world.Ops[0].Uuid;
            string worldName = $"{world.Ops[0].Name}'s server";
            List<PlayerResponse> whitelistedPlayers = [];

            foreach (var player in whitelist.Result)
            {
                PlayerResponse whitelistedPlayer = new()
                {
                    Name = player.Name,
                    Uuid = player.Uuid,
                    Accepted = true,
                    Online = false,
                    Operator = whitelist.Ops.Find(p => p.Name == player.Name) != null,
                    Permission = whitelist.Ops.Find(p => p.Name == player.Name) != null ? "OPERATOR" : "MEMBER",
                };

                whitelistedPlayers.Add(whitelistedPlayer);
            }

            string cookie = Request.Headers.Cookie;
            string gameVerision = cookie.Split(";")[2].Split("=")[1];

            int versionsCompared = SemVersion.Parse(gameVerision, SemVersionStyles.OptionalPatch).ComparePrecedenceTo(SemVersion.Parse(world.GameVersion, SemVersionStyles.OptionalPatch));
            string isCompatible = versionsCompared == 0 ? "COMPATIBLE" : versionsCompared < 0 ? "NEEDS_DOWNGRADE" : "NEEDS_UPGRADE";

            WorldResponse response = new()
            {
                Id = wId,
                Owner = worldOwnerName,
                OwnerUUID = worldOwnerUuid,
                Name = worldName,
                Motd = world.Motd.Replace("�", "§"),
                State = whitelist.Enabled ? "CLOSED" : "OPEN",
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
                ActiveVersion = world.GameVersion,
                Slots =
                [
                    new()
                    {
                        SlotId = 1,
                        Options = JsonConvert.SerializeObject(new
                        {
                            slotName = "my-mc.link",
                            version = world.GameVersion,
                            compatibility = isCompatible,
                        })
                    }
                ]
            };

            return Ok(response);
        }

        [HttpPost("{wId}")]
        [CheckRealmOwner]
        public ActionResult<string> UpdateRealms(int wId)
        {
            ErrorResponse errorResponse = new()
            {
                ErrorCode = 400,
                ErrorMsg = "You can change the MOTD trough server.properties file"
            };

            return BadRequest(errorResponse);
        }

        [HttpPost("{wId}/reset")]
        [CheckRealmOwner]
        public ActionResult<string> ChangeSlot(int wId)
        {
            ErrorResponse errorResponse = new()
            {
                ErrorCode = 400,
                ErrorMsg = "lol nice try"
            };

            return BadRequest(errorResponse);
        }

        [HttpPut("{wId}/open")]
        [CheckRealmOwner]
        public async Task<ActionResult<bool>> Open(int wId)
        {
            var _api = new MyMcAPI.Wrapper(Environment.GetEnvironmentVariable("MYMC_API_KEY"));

            var world = (await _api.GetAllServers()).Servers[wId];
            var api = new MyMcAPI.Wrapper(world.OwnersToken);

            if (world == null)
            {
                ErrorResponse errorResponse = new()
                {
                    ErrorCode = 404,
                    ErrorMsg = "World not found"
                };

                return NotFound(errorResponse);
            }

            api.ExecuteCommand("whitelist off");

            return Ok(true);
        }

        [HttpPut("{wId}/close")]
        [CheckRealmOwner]
        public async Task<ActionResult<bool>> Close(int wId)
        {
            var _api = new MyMcAPI.Wrapper(Environment.GetEnvironmentVariable("MYMC_API_KEY"));

            var world = (await _api.GetAllServers()).Servers[wId];
            var api = new MyMcAPI.Wrapper(world.OwnersToken);

            if (world == null)
            {
                ErrorResponse errorResponse = new()
                {
                    ErrorCode = 404,
                    ErrorMsg = "World not found"
                };

                return NotFound(errorResponse);
            }

            api.ExecuteCommand("whitelist on");

            return Ok(true);
        }

        [HttpPost("{wId}/slot/{sId}")]
        [CheckRealmOwner]
        public ActionResult<string> UpdateSlot(int wId, int sId)
        {
            ErrorResponse errorResponse = new()
            {
                ErrorCode = 400,
                ErrorMsg = "no."
            };

            return BadRequest(errorResponse);
        }

        [HttpGet("{wId}/slot/{sId}/download")]
        [CheckRealmOwner]
        public ActionResult<string> GetBackups(int wId, int sId)
        {
            ErrorResponse errorResponse = new()
            {
                ErrorCode = 400,
                ErrorMsg = "Wouldn't it be nice if you could download your world to singleplayer? Well I think that too"
            };

            return BadRequest(errorResponse);
        }

        [HttpGet("v1/{wId}/join/pc")]
        public async Task<ActionResult<ConnectionResponse>> Join(int wId)
        {
            AllServersResponse AllServers = await new MyMcAPI.Wrapper(Environment.GetEnvironmentVariable("MYMC_API_KEY")).GetAllServers();

            ConnectionResponse connection = new()
            {
                Address = AllServers.Servers[wId].Connect,
                PendingUpdate = false
            };

            return Ok(connection);
        }
    }
}
