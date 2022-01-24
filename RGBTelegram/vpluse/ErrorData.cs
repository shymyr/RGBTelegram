using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.vpluse
{
    public class ErrorData
    {
        public IList<Data> data { get; set; }
        public int status { get; set; }
        public bool success { get; set; }
    }
}
