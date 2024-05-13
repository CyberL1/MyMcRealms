﻿using Microsoft.AspNetCore.Mvc;
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

            string gameVerision = cookie.Split(";")[2].Split("=")[1];

            List<WorldResponse> allWorlds = [];

            AllServersResponse AllServers = await new MyMcAPI.MyMcAPI(Environment.GetEnvironmentVariable("MYMC_API_KEY")).GetAllServers();

            foreach (var world in AllServers.Servers)
            {
                if (world.Online)
                {
                    int versionsCompared = SemVersion.Parse(gameVerision, SemVersionStyles.OptionalPatch).ComparePrecedenceTo(SemVersion.Parse(world.GameVersion, SemVersionStyles.OptionalPatch));
                    string isCompatible = versionsCompared == 0 ? "COMPATIBLE" : versionsCompared < 0 ? "NEEDS_DOWNGRADE" : "NEEDS_UPGRADE";

                    bool isOlderVersion = SemVersion.Parse(gameVerision, SemVersionStyles.OptionalPatch).ComparePrecedenceTo(SemVersion.Parse("1.20.3", SemVersionStyles.OptionalPatch)) < 0;
                    string isCompatibleOnOlderVersions = (isOlderVersion && isCompatible.StartsWith("NEEDS_")) ? "CLOSED" : "OPEN";

                    string worldOwnerName = world.Ops.ToArray().Length == 0 ? "Owner" : world.Ops[0].Name;
                    string worldOwnerUuid = world.Ops.ToArray().Length == 0 ? "069a79f444e94726a5befca90e38aaf5" : world.Ops[0].Uuid;

                    WorldResponse response = new()
                    {
                        Id = AllServers.Servers.IndexOf(world),
                        Owner = worldOwnerName,
                        OwnerUUID = worldOwnerUuid,
                        Name = world.ServerName,
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
            return NotFound("World not found");
        }

//        [HttpPut("{id}/open")]
//        public async Task<ActionResult<bool>> Open(int id)
//        {
//            var world = worlds.Find(w => w.Id == id);

//            if (world == null) return NotFound("World not found");

            // Turn off whitelist

//            return Ok(true);
//        }

//        [HttpPut("{id}/close")]
//        public async Task<ActionResult<bool>> Close(int id)
//        {
//            var world = worlds.FirstOrDefault(w => w.Id == id);

//            if (world == null) return NotFound("World not found");

            // Turn on whitelist

//            return Ok(true);
//        }

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
