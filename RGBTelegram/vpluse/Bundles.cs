using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.vpluse
{
    public class Bundles
    {
        public Bundles()
        {
            this.gifts = new List<Gifts>();
            this.attempts = new List<Attempts>();
            this.error = new ErrorData();
            this.status = 0;
            this.success = false;
            this.message = null;
        }
        public List<Gifts> gifts { get; set; }
        public List<Attempts> attempts { get; set; }
        public ErrorData error { get; set; }
        public int status { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }
}
