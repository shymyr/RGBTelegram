using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Entities
{
    public class UZSession:BaseEntity
    {
        [JsonIgnore]
        public UZUser User { get; set; }
        public long? UserId { get; set; }
        public DateTime dateTime { get; set; } = DateTime.UtcNow;
        public UZOperType Type { get; set; }
        public Language language { get; set; }
    }
}
