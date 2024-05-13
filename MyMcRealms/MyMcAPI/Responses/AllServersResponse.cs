namespace MyMcRealms.MyMcAPI.Responses
{
    public class AllServersResponse
    {
        public bool Success { get; set; }
        public List<Server> Servers { get; set; }
    }

    public class Server
    {
        public string ServerName { get; set; } = string.Empty;
        public string Connect { get; set; } = string.Empty;
        public string GameVersion { get; set; } = string.Empty;
        public string Motd { get; set; } = string.Empty;
        public bool Online { get; set; }
        public List<UserCache> UserCache { get; set; } = null!;
        public List<Op> Ops { get; set; } = null!;
    }

    public class UserCache
    {
        public string Name { get; set; } = null!;
        public string Uuid { get; set; } = null!;
        public string ExpiresOn { get; set; } = null!;
    }

    public class Op
    {
        public string Uuid { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Level { get; set; }
        public bool BypassesPlayerLimit { get; set; }
    }
}
