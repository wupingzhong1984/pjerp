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
    
    public partial class base_menu
    {
        public int id { get; set; }
        public string menu_name { get; set; }
        public string menu_url { get; set; }
        public Nullable<int> menu_parent_id { get; set; }
        public Nullable<int> menu_level { get; set; }
        public Nullable<int> menu_sort { get; set; }
        public Nullable<int> menu_is_view { get; set; }
        public Nullable<int> menu_is_frame_view { get; set; }
        public string menu_class { get; set; }
        public string menu_class_icon { get; set; }
        public string menu_actions { get; set; }
        public Nullable<int> createdby_id { get; set; }
        public string createdby_name { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<int> deleteflag { get; set; }
    }
}
