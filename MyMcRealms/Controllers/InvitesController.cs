using Microsoft.AspNetCore.Mvc;
using MyMcRealms.Responses;
using MyMcRealms.Attributes;
using MyMcRealms.Requests;

namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class InvitesController : ControllerBase
    {
        [HttpPost("{wId}")]
        public async Task<ActionResult<WorldResponse>> InvitePlayer(int wId, PlayerRequest body)
        {
            string cookie = Request.Headers.Cookie;
            string playerName = cookie.Split(";")[1].Split("=")[1];

            if (body.Name == playerName) return Forbid("You cannot invite yourself");

            var _api = new MyMcAPI.Wrapper(Environment.GetEnvironmentVariable("MYMC_API_KEY"));
            var world = (await _api.GetAllServers()).Servers[wId];

            if (world == null) return NotFound("World not found");

            // Get player name
            var playerInfo = await new HttpClient().GetFromJsonAsync<MinecraftPlayerInfo>($"https://api.mojang.com/users/profiles/minecraft/{body.Name}");

            if (world.Whitelist.Any(p => p.Name == body.Name)) return BadRequest("Player already whitelisted");

            var api = new MyMcAPI.Wrapper(world.OwnersToken);
            api.ExecuteCommand($"whitelist add {body.Name}");

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

            Player npl = new()
            {
                Name = body.Name,
                Uuid = playerInfo.Id,
                Accepted = true,
                Online = false,
                Operator = world.Ops.Find(p => p.Name == body.Name) != null,
                Permission = world.Ops.Find(p => p.Name == body.Name) != null ? "OPERATOR" : "MEMBER",
            };

            whitelistedPlayers.Add(npl);

            WorldResponse response = new()
            {
                Id = wId,
                Owner = "blank",
                OwnerUUID = "blank",
                Name = "blank",
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

        [HttpDelete("{wId}/invite/{uuid}")]
        public async Task<ActionResult<bool>> DeleteInvite(int wId, string uuid)
        {
            var _api = new MyMcAPI.Wrapper(Environment.GetEnvironmentVariable("MYMC_API_KEY"));
            var world = (await _api.GetAllServers()).Servers[wId];

            if (world == null) return NotFound("World not found");

            var player = world.Whitelist.Find(p => p.Uuid.Replace("-", "") == uuid);

            // Get player name
            var playerInfo = await new HttpClient().GetFromJsonAsync<MinecraftPlayerInfo>($"https://sessionserver.mojang.com/session/minecraft/profile/{uuid}");

            if (!world.Whitelist.Any(p => p.Uuid.Replace("-", "") == uuid)) return BadRequest("Player not whitelisted");

            var api = new MyMcAPI.Wrapper(world.OwnersToken);
            api.ExecuteCommand($"whitelist remove {player.Name}");

            return Ok(true);
        }
    }
}