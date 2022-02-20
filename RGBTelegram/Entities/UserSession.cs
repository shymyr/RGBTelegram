using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RGBTelegram.Entities
{
    public class UserSession : BaseEntity
    {
        [JsonIgnore]
        public AppUser User { get; set; }
        public bool Authorized { get; set; }
        public long? UserId { get; set; }
        public DateTime dateTime { get; set; } = DateTime.UtcNow;
        public OperationType Type { get; set; }
        public Country country { get; set; }
        public Language language { get; set; } 
    }
}
