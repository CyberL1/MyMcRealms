﻿using Microsoft.AspNetCore.Mvc;
using MyMcRealms.Responses;
using MyMcRealms.Attributes;

namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class OpsController : ControllerBase
    {
        [HttpPost("{wId}/{uuid}")]
        [CheckRealmOwner]
        public async Task<ActionResult<OpsResponse>> OpPlayer(int wId, string uuid)
        {
            var _api = new MyMcAPI.Wrapper(Environment.GetEnvironmentVariable("MYMC_API_KEY"));
            var world = (await _api.GetAllServers()).Servers[wId];
            
            var api = new MyMcAPI.Wrapper(world.OwnersToken);
            var whitelist = await api.GetWhitelist();

            var ops = whitelist.Ops;
            var player = whitelist.Result.Find(p => p.Uuid.Replace("-", "") == uuid);

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
        [CheckRealmOwner]
        public async Task<ActionResult<OpsResponse>> DeopPlayerAsync(int wId, string uuid)
        {
            var _api = new MyMcAPI.Wrapper(Environment.GetEnvironmentVariable("MYMC_API_KEY"));
            var world = (await _api.GetAllServers()).Servers[wId];

            var api = new MyMcAPI.Wrapper(world.OwnersToken);
            var whitelist = await api.GetWhitelist();

            var ops = whitelist.Ops;
            var player = whitelist.Result.Find(p => p.Uuid.Replace("-", "") == uuid);

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