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
    
    public partial class base_aciton
    {
        public int id { get; set; }
        public string action_name { get; set; }
        public string action_class { get; set; }
        public string action_desc { get; set; }
        public Nullable<int> action_type { get; set; }
        public string action_en { get; set; }
        public Nullable<int> createdby_id { get; set; }
        public string createdby_name { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<int> deleteflag { get; set; }
    }
}