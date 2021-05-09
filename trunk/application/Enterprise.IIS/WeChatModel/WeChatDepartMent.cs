using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Enterprise.IIS.WeChatModel
{
    public class WeChatDepartMent
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }

        public List<WeChatDepartMents> department { get; set; }
    }

    public class WeChatDepartMents {
        public int id { get; set; }

        public string name { get; set; }

        public int parentid { get; set; }

        public int order { get; set; }

    }
}