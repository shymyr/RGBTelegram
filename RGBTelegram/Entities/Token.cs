using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Entities
{
    public class Token : BaseEntity
    {
        public string AuthToken { get; set; }
        public DateTime Expired { get; set; }
    }
}
