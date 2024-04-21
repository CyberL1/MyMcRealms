
namespace MyMcRealms.MyMcAPI.Responses
{
    public class AllServersResponse
    {
        public bool Success { get; set; }
        public List<Server> Servers { get; set; }

        public static implicit operator Task<object>(AllServersResponse? v)
        {
            throw new NotImplementedException();
        }
    }

    public class Server
    {
        public string ServerName { get; set; } = string.Empty;
        public string Connect { get; set; } = string.Empty;
        public string GameVersion {  get; set; } = string.Empty;
    }
}
