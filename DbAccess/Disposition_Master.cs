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
    
    public partial class Disposition_Master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Disposition_Master()
        {
            this.Patient_Status = new HashSet<Patient_Status>();
        }
    
        public int Disposition_Id { get; set; }
        public string Disposition { get; set; }
        public string Disposition_Group { get; set; }
        public Nullable<int> Created_By_User { get; set; }
        public Nullable<int> Updated_By_User { get; set; }
        public Nullable<System.DateTime> Create_Dt_Time { get; set; }
        public Nullable<System.DateTime> Update_Dt_Time { get; set; }
        public bool IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Patient_Status> Patient_Status { get; set; }
    }
}
