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
    
    public partial class base_bulletin
    {
        public int id { get; set; }
        public string title { get; set; }
        public string m_content { get; set; }
        public Nullable<System.DateTime> pubdate { get; set; }
        public Nullable<int> isrepeal { get; set; }
        public Nullable<System.DateTime> repealdate { get; set; }
        public Nullable<System.DateTime> deletedate { get; set; }
        public Nullable<System.DateTime> validity_s { get; set; }
        public Nullable<System.DateTime> validity_e { get; set; }
        public Nullable<int> precedence { get; set; }
        public Nullable<int> receiver { get; set; }
        public string sequence_id { get; set; }
        public Nullable<int> deleteflag { get; set; }
        public string url { get; set; }
        public Nullable<int> FCompanyId { get; set; }
        public Nullable<int> FFlag { get; set; }
    }
}
