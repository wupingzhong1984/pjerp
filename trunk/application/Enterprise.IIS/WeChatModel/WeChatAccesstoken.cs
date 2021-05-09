using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Enterprise.IIS.WeChatModel
{
    public class WeChatAccesstoken
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }

        public string access_token { get; set; }

        public int expires_in { get; set; }

        public DateTime LogTime { get; set; }

    }
}