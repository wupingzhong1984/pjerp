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
    
    public partial class base_permission
    {
        public int id { get; set; }
        public Nullable<int> role_id { get; set; }
        public string orgnization_ids { get; set; }
        public string supplier_ids { get; set; }
        public string storage_ids { get; set; }
        public string customer_ids { get; set; }
        public Nullable<int> deleteflag { get; set; }
        public Nullable<int> FCompanyId { get; set; }
        public Nullable<int> FFlag { get; set; }
    }
}