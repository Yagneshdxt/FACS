//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DbAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Address_Master
    {
        public int Address_Id { get; set; }
        public int Address_Type { get; set; }
        public int Address_Sub_Type { get; set; }
        public string Address_Line_1 { get; set; }
        public string Address_Line_2 { get; set; }
        public string Address_Line_3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int Created_By_User { get; set; }
        public int Updated_By_User { get; set; }
        public System.DateTime Create_Dt_Time { get; set; }
        public System.DateTime Update_Dt_Time { get; set; }
    
        public virtual Address_Sub_Type_Master Address_Sub_Type_Master { get; set; }
    }
}
