namespace MyMcRealms.Responses
{
    public class SlotResponse
    {
        public int SlotId { get; set; }
        public List<string> Settings { get; set; } = [];
        public string Options { get; set; } = null!;
    }
}