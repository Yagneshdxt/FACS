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
    
    public partial class Client_Master
    {
        public int Hospital_Id { get; set; }
        public string Hospital_Name { get; set; }
        public Nullable<int> Hospital_Group_Id { get; set; }
        public string Hospital_Speciality { get; set; }
        public Nullable<int> Number_Of_Beds { get; set; }
        public Nullable<bool> ICCU_Available { get; set; }
        public string Visitor_Hours { get; set; }
        public int Create_By_User { get; set; }
        public int Updated_By_User { get; set; }
        public System.DateTime Create_Dt_Time { get; set; }
        public System.DateTime Update_Dt_Time { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Client_Group_Master Client_Group_Master { get; set; }
    }
}
