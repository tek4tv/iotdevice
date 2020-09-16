using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tek4TV.Devices
{
    public class RequestBodyVideo
    {

        public int[] ListCategory { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string QueryString { get; set; }

    }
}