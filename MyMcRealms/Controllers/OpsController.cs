using Microsoft.AspNetCore.Mvc;
using Minecraft_Realms_Emulator.Responses;
using MyMcRealms.Attributes;
using MyMcRealms.MyMcAPI;

namespace Minecraft_Realms_Emulator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class OpsController : ControllerBase
    {
        [HttpPost("{wId}/{uuid}")]
        public async Task<ActionResult<OpsResponse>> OpPlayer(int wId, string uuid)
        {
            var _api = new MyMcAPI(Environment.GetEnvironmentVariable("MYMC_API_KEY"));
            var world = (await _api.GetAllServers()).Servers[wId];
            var api = new MyMcAPI(world.OwnersToken);

            var ops = world.Ops;
            var player = world.Whitelist.Find(p => p.Uuid.Replace("-", "") == uuid);

            List<string> opNames = [];

            foreach (var op in ops)
            {
                opNames.Add(op.Name);
            }
            
            api.ExecuteCommand($"op {player.Name}");
            opNames.Add(player.Name);

            var opsResponse = new OpsResponse
            {
                Ops = opNames
            };

            return Ok(opsResponse);
        }

        [HttpDelete("{wId}/{uuid}")]
        public async Task<ActionResult<OpsResponse>> DeopPlayerAsync(int wId, string uuid)
        {
            var _api = new MyMcAPI(Environment.GetEnvironmentVariable("MYMC_API_KEY"));
            var world = (await _api.GetAllServers()).Servers[wId];
            var api = new MyMcAPI(world.OwnersToken);

            var ops = world.Ops;
            var player = world.Whitelist.Find(p => p.Uuid.Replace("-", "") == uuid);

            List<string> opNames = [];

            foreach (var op in ops)
            {
                opNames.Add(op.Name);
            }

            api.ExecuteCommand($"deop {player.Name}");
            opNames.Remove(player.Name);

            var opsResponse = new OpsResponse
            {
                Ops = opNames
            };

            return Ok(opsResponse);
        }
    }
}