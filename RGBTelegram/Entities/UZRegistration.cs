using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Entities
{
    public class UZRegistration: BaseEntity
    {
        public long ChatId { get; set; }
        public string phone { get; set; }
        public string region_id { get; set; }
        public int city_id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string middle_name { get; set; }
        public string birthdate { get; set; }
    }
}
