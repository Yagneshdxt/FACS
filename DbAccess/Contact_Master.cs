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
    
    public partial class Contact_Master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contact_Master()
        {
            this.Patient_Status = new HashSet<Patient_Status>();
        }
    
        public int Contact_Id { get; set; }
        public int Contact_Type { get; set; }
        public int Contact_Sub_Type { get; set; }
        public string Country_Code { get; set; }
        public string City_Code { get; set; }
        public string Phone_1 { get; set; }
        public string Phone_2 { get; set; }
        public string Phone_3 { get; set; }
        public string Phone_4 { get; set; }
        public string Phone_5 { get; set; }
        public string Cell_1 { get; set; }
        public string Cell_2 { get; set; }
        public string Fax_1 { get; set; }
        public string Fax_2 { get; set; }
        public string Email_1 { get; set; }
        public string Email_2 { get; set; }
        public string Contact_Person_Name { get; set; }
        public Nullable<int> Created_By_User { get; set; }
        public Nullable<int> Updated_By_User { get; set; }
        public Nullable<System.DateTime> Create_Dt_Time { get; set; }
        public Nullable<System.DateTime> Update_Dt_Time { get; set; }
    
        public virtual Contact_Sub_Type_Master Contact_Sub_Type_Master { get; set; }
        public virtual Contact_Type_Master Contact_Type_Master { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Patient_Status> Patient_Status { get; set; }
    }
}
