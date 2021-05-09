using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Enterprise.IIS.Common
{
    public class TmsReturnModel
    {
        public string code { get; set; }

        public string msg { get; set; }

        public string serial { get; set; }

        public object data { get; set; }
    }
}