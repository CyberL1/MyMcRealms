namespace MyMcRealms.Responses
{
    public class WorldResponse
    {
        public int Id { get; set; }
        public string? Owner { get; set; }
        public string? OwnerUUID { get; set; }
        public string? Name { get; set; }
        public string? Motd { get; set; }
        public int GameMode { get; set; }
        public bool IsHardcore { get; set; }
        public string State { get; set; } = "OPEN";
        public string WorldType { get; set; } = "NORMAL";
        public List<PlayerResponse> Players { get; set; } = [];
        public int MaxPlayers { get; set; } = 10;
        public string? MinigameName { get; set; }
        public int? MinigameId { get; set; }
        public string? MinigameImage { get; set; }
        public int ActiveSlot { get; set; } = 1;
        public List<SlotResponse> Slots { get; set; } = [];
        public bool Member { get; set; } = false;
        public string RemoteSubscriptionId { get; set; } = Guid.NewGuid().ToString();
        public int DaysLeft { get; set; } = 30;
        public bool Expired { get; set; } = false;
        public bool ExpiredTrial { get; set; } = false;
        public string Compatibility { get; set; } = string.Empty;
        public string ActiveVersion { get; set; } = string.Empty;
    }
}