using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Enterprise.IIS.WeChatModel
{
    public class WeChatEmployee
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }

        public List<Employee> userlist { get; set; }

    }

    public class Employee
    {
        public string userid { get; set; }
        public string name { get; set; }

        public Array department { get; set; }
    }

}