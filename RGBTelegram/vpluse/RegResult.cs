using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.vpluse
{
    public class RegResult
    {
       public string phone { get; set; }
        public string iin { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public string family_stat { get; set; }
        public DateTime birthday { get; set; }
        public int sms_id { get; set; }
        public int sms_expired { get; set; }
    }
}
