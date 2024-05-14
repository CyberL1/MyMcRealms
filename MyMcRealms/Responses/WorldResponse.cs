using System.Text.Json;

namespace MyMcRealms.Responses
{
    public class WorldResponse
    {
        public int Id { get; set; }
        //        public Subscription? Subscription { get; set; }
        public string? Owner { get; set; }
        public string? OwnerUUID { get; set; }
        public string? Name { get; set; }
        public string? Motd { get; set; }
        public string State { get; set; } = "OPEN";
        public string WorldType { get; set; } = "NORMAL";
        public List<Player> Players { get; set; } = [];
        public int MaxPlayers { get; set; } = 10;
        public string? MinigameName { get; set; }
        public int? MinigameId { get; set; }
        public string? MinigameImage { get; set; }
        public int ActiveSlot { get; set; } = 1;
        public JsonDocument[] Slots { get; set; } = [];
        public bool Member { get; set; } = false;
        public string RemoteSubscriptionId { get; set; } = Guid.NewGuid().ToString();
        public int DaysLeft { get; set; } = 30;
        public bool Expired { get; set; } = false;
        public bool ExpiredTrial { get; set; } = false;
        public string Compatibility { get; set; } = string.Empty;
        public string ActiveVersion { get; set; } = string.Empty;
    }

    public class Player
    {
        public string Name { get; set; } = string.Empty;
        public string Uuid { get; set; } = string.Empty;
        public bool Operator { get; set; }
        public bool Accepted { get; set; }
        public bool Online { get; set; }
        public string Permission { get; set; } = "MEMBER";
    }
}