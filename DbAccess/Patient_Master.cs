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
    
    public partial class Patient_Master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient_Master()
        {
            this.Patient_Receivables_Info = new HashSet<Patient_Receivables_Info>();
            this.Patient_Treatments = new HashSet<Patient_Treatments>();
            this.Patient_Status = new HashSet<Patient_Status>();
            this.Payments = new HashSet<Payment>();
        }

        public int Patient_Id { get; set; }
        public string PatientNoFromClient { get; set; }
        public string Patient_Last_Name { get; set; }
        public string Patient_First_Name { get; set; }
        public string Patient_Middle_Name { get; set; }
        public string Patient_SocialSecurity { get; set; }
        public int? Hospital_Id { get; set; }
        public Nullable<System.DateTime> Admit_Date { get; set; }
        public Nullable<System.DateTime> Discharge_Date { get; set; }
        public decimal? Patient_Bill_Amount { get; set; }
        public decimal? Patient_Insurance_Bill_Amount { get; set; }
        public decimal? Total_Charges { get; set; }
        public Nullable<System.DateTime> Placement_Date { get; set; }
        public int Created_By_User { get; set; }
        public int Updated_By_User { get; set; }
        public System.DateTime Create_Dt_Time { get; set; }
        public Nullable<System.DateTime> Update_Dt_Time { get; set; }
        public bool IsActive { get; set; }
        public bool IsClosed { get; set; }
        public Nullable<System.DateTime> Closed_Date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Patient_Receivables_Info> Patient_Receivables_Info { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Patient_Treatments> Patient_Treatments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Patient_Status> Patient_Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
