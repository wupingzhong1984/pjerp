//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Enterprise.Service.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class base_role
    {
        public int id { get; set; }
        public string role_name { get; set; }
        public string role_desc { get; set; }
        public Nullable<int> role_sort { get; set; }
        public string role_menu_code_p { get; set; }
        public string role_menu_code { get; set; }
        public string role_action { get; set; }
        public Nullable<int> role_flag { get; set; }
        public Nullable<int> permission_flag { get; set; }
        public Nullable<int> account_id { get; set; }
        public Nullable<int> createdby_id { get; set; }
        public string createdby_name { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<int> deleteflag { get; set; }
        public Nullable<int> FCompanyId { get; set; }
        public Nullable<int> FFlag { get; set; }
    }
}
