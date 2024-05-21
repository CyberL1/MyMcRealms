using MyMcRealms.Attributes;
using MyMcRealms.MyMcAPI.Responses;

namespace Minecraft_Realms_Emulator.Middlewares
{
    public class CheckRealmOwnerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            var attribute = endpoint?.Metadata.GetMetadata<CheckRealmOwnerAttribute>();

            if (attribute == null)
            {
                await _next(httpContext);
                return;
            }

            string playerUUID = httpContext.Request.Headers.Cookie.ToString().Split(";")[0].Split(":")[2];

            var servers = await new MyMcRealms.MyMcAPI.Wrapper(Environment.GetEnvironmentVariable("MYMC_API_KEY")).GetAllServers();
            Server server = servers.Servers.Find(s => servers.Servers.IndexOf(s) == int.Parse(httpContext.Request.RouteValues["wId"].ToString()));

            if (server == null)
            {
                httpContext.Response.StatusCode = 404;
                await httpContext.Response.WriteAsync("World not found");
                return;
            }

            if (server.Ops.Count == 0) {
                httpContext.Response.StatusCode = 403;
                await httpContext.Response.WriteAsync("This world isn't owned by anyone");
                return;
            }

            if (!attribute.IsRealmOwner(playerUUID, server.Ops[0].Uuid))
            {
                httpContext.Response.StatusCode = 403;
                await httpContext.Response.WriteAsync("You don't own this world");
                return;
            }

            await _next(httpContext);
        }
    }
}
