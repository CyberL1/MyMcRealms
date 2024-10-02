using Microsoft.AspNetCore.Mvc;
using MyMcRealms.Attributes;
using MyMcRealms.Helpers;
using MyMcRealms.MyMcAPI;
using MyMcRealms.Responses;
using Newtonsoft.Json;

namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class ActivitiesController : ControllerBase
    {
        [HttpGet("liveplayerlist")]
        public async Task<ActionResult<LivePlayerListsResponse>> GetLivePlayerList()
        {
            string cookie = Request.Headers.Cookie;
            string playerUUID = cookie.Split(";")[0].Split(":")[2];

            List<LivePlayerList> lists = [];

            var allServers = await new Wrapper(Environment.GetEnvironmentVariable("MYMC_API_KEY")).GetAllServers();

            foreach (var world in allServers.Servers)
            {
                if (world.WhitelistEnable && !(world.Whitelist.Any(p => p.Uuid.Replace("-", "") == playerUUID) || world.Ops.Any(p => p.Uuid.Replace("-", "") == playerUUID)))
                {
                    continue;
                }

                var query = new MinecraftServerQuery().Query(world.Connect);

                if (query == null) continue;

                List<object> players = [];

                if (query.Players.Sample == null) continue;

                foreach (var player in query.Players.Sample)
                {
                    players.Add(new
                    {
                        playerId = player.Id.Replace("-", ""),
                    });
                }

                LivePlayerList list = new()
                {
                    ServerId = allServers.Servers.IndexOf(world),
                    PlayerList = JsonConvert.SerializeObject(players),
                };

                lists.Add(list);
            };

            LivePlayerListsResponse response = new()
            {
                Lists = lists
            };

            return Ok(response);
        }
    }
}
