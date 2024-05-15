namespace MyMcRealms.Responses
{
    public class PlayerResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Uuid { get; set; } = string.Empty;
        public bool Operator { get; set; }
        public bool Accepted { get; set; }
        public bool Online { get; set; }
        public string Permission { get; set; } = "MEMBER";
    }
}