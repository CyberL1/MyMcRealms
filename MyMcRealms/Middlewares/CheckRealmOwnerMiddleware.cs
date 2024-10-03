using Minecraft_Realms_Emulator.Responses;
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
                ErrorResponse errorResponse = new()
                {
                    ErrorCode = 404,
                    ErrorMsg = "World not found"
                };

                httpContext.Response.StatusCode = 404;
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
                return;
            }

            if (server.Ops.Count == 0) {
                ErrorResponse errorResponse = new()
                {
                    ErrorCode = 403,
                    ErrorMsg = "This world isn't owned by anyone"
                };

                httpContext.Response.StatusCode = 403;
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
                return;
            }

            if (!attribute.IsRealmOwner(playerUUID, server.Ops[0].Uuid.Replace("-", "")))
            {
                ErrorResponse errorResponse = new()
                {
                    ErrorCode = 403,
                    ErrorMsg = "You don't own this world"
                };

                httpContext.Response.StatusCode = 403;
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
                return;
            }

            await _next(httpContext);
        }
    }
}
