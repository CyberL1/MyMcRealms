using System.Text.Json;

namespace MyMcRealms.Entities
{
    public class Backup
    {
        public int Id { get; set; }
        public World World { get; set; }
        public string BackupId { get; set; }
        public long LastModifiedDate { get; set; }
        public int Size { get; set; }
        public JsonDocument Metadata { get; set; }
    }
}
