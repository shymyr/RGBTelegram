using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.vpluse
{
    public class SignUp
    {
        public RegData RegData { get; set; }
        public int status { get; set; }
        public bool success { get; set; }
        public string field { get; set; }
        public string message { get; set; }
    }
}
