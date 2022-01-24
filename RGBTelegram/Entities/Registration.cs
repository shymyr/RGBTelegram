using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Entities
{
    public class Registration: BaseEntity
    {
        public string phone { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middlename { get; set; }
        public string gender { get; set; }
        public string family_stat { get; set; }
        public string birth_day { get; set; }
        public string email { get; set; }
        public int city_id { get; set; }
        public int region_id { get; set; }
        public string iin { get; set; }
    }
}
