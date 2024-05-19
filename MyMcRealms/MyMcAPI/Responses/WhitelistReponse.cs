namespace MyMcRealms.MyMcAPI.Responses
{
    public class WhitelistReponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public List<Whitelist> Result { get; set; } = null!;
        public bool Enabled { get; set; }
        public List<Op> Ops { get; set; } = null!;
    }
}