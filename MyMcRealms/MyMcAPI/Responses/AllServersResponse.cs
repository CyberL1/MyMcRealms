namespace MyMcRealms.MyMcAPI.Responses
{
    public class AllServersResponse
    {
        public bool Success { get; set; }
        public List<Server> Servers { get; set; } = null!;
    }

    public class Server
    {
        public string ServerName { get; set; } = string.Empty;
        public string Connect { get; set; } = string.Empty;
        public string GameVersion { get; set; } = string.Empty;
        public string Motd { get; set; } = string.Empty;
        public bool Online { get; set; }
        public List<Op> Ops { get; set; } = null!;
        public List<Ban> Banlist { get; set; } = null!;
        public List<Whitelist> Whitelist { get; set; } = null!;
        public bool WhitelistEnable { get; set; }
        public string OwnersToken { get; set; } = string.Empty;
        public string Gamemode { get; set; } = null!;
    }

    public class Op
    {
        public string Uuid { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Level { get; set; }
        public bool BypassesPlayerLimit { get; set; }
    }

    public class Ban
    {
        public string Uuid { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Created { get; set; } = null!;
        public string Source { get; set; } = null!;
        public string Expires { get; set; } = null!;
        public string Reason { get; set; } = null!;
    }

    public class Whitelist
    {
        public string Uuid { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
