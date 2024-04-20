using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMcRealms.Entities
{
    [PrimaryKey(nameof(Key))]
    public class Configuration
    {
        public string Key { get; set; } = string.Empty;
        [Column(TypeName = "jsonb")]
        public dynamic Value { get; set; } = string.Empty;
    }
}
