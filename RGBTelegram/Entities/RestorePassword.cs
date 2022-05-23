using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Entities
{
    public class RestorePassword : BaseEntity
    {
        public long ChatID { get; set; }
        public string phone { get; set; }
        public string sms_code { get; set; }
        public string new_password { get; set; }
    }
}
