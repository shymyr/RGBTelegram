using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Entities
{
    public class AuthData:BaseEntity
    {
        public long ChatId { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
    }
}
