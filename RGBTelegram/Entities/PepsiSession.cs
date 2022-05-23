using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Entities
{
    public class PepsiSession : BaseEntity
    {
        [JsonIgnore]
        public PepsiUser User { get; set; }
        public bool Authorized { get; set; }
        public long? UserId { get; set; }
        public DateTime dateTime { get; set; } = DateTime.UtcNow;
        public OperationType Type { get; set; }
        public Country country { get; set; }
        public Language language { get; set; }
        public string Token { get; set; }
        public double? expire { get; set; }
    }
}
