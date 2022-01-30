using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.vpluse
{
    public class Family
    {
        public List<Item> Items { get; set; }

        public int status { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }
}
