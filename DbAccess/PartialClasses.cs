using System.ComponentModel.DataAnnotations;


namespace DbAccess
{
    class PartialClasses
    {

    }

    [MetadataType(typeof(ClientGroupMaster_Meta))]
    public partial class Client_Group_Master
    {
    }
    [MetadataType(typeof(PatientTypeMaster_Meta))]
    public partial class Patient_Type_Master {
        
    }
    [MetadataType(typeof(PatientReceivablesInfo_Meta))]
    public partial class Patient_Receivables_Info {

    }

    [MetadataType(typeof(Payment_Meta))]
    public partial class Payment {
        /*
                 public int Payments_Id { get; set; }
        public int Patient_Id { get; set; }
        public int? Payment_Index { get; set; }
        public string Payment_Type { get; set; }
        public decimal? Payment_Amount { get; set; }
        public Nullable<System.DateTime> Payment_Date { get; set; }
        public Nullable<System.DateTime> Payment_Post_Date { get; set; }
        public string Payment_Details { get; set; }
        public Nullable<int> Created_By_User { get; set; }
        public Nullable<int> Updated_By_User { get; set; }
        public Nullable<System.DateTime> Create_Dt_Time { get; set; }
        public Nullable<System.DateTime> Update_Dt_Time { get; set; }
        public bool IsActive { get; set; }
        public decimal? Revenue { get; set; }
         */
    }

    [MetadataType(typeof(PatientStatus_Meta))]
    public partial class Patient_Status {
        /*
         public int Status_Id { get; set; }
        public int Patient_Id { get; set; }
        public int Collector_Id { get; set; }
        public int Disposition_Id { get; set; }
        public Nullable<System.DateTime> Contact_Date { get; set; }
        public string Method_Of_Contact { get; set; }
        public string Valid_Contact_Number { get; set; }
        public string Valid_Contact_Number_Type { get; set; }
        public decimal? Current_Balance { get; set; }
        public string Notes { get; set; }
        public bool IsLatest { get; set; }
        public Nullable<int> Created_By_User { get; set; }
        public Nullable<int> Updated_By_User { get; set; }
        public Nullable<System.DateTime> Create_Dt_Time { get; set; }
        public Nullable<System.DateTime> Uppdate_Dt_Time { get; set; }
        public Nullable<System.DateTime> Last_Payment_Date { get; set; }
        public Nullable<decimal> Last_Payment_Amount { get; set; }
        public Nullable<decimal> Total_Paid_Amount { get; set; }
         */

    }

    [MetadataType(typeof(PatientTreatments_Meta))]
    public partial class Patient_Treatments {
        /*
                 public int Patient_Treatments_Id { get; set; }
        public int Patient_Id { get; set; }
        public Nullable<System.DateTime> Treatment_Date { get; set; }
        public string Treatment_Code { get; set; }
        public string Treatment_Description { get; set; }
        public decimal Total_Charges { get; set; }
        public Nullable<int> Created_By_User { get; set; }
        public Nullable<int> Updated_By_User { get; set; }
        public Nullable<System.DateTime> Create_Dt_Time { get; set; }
        public Nullable<System.DateTime> Update_Dt_Time { get; set; }
         */
    }


    [MetadataType(typeof(PatientMaster_Meta))]
    public partial class Patient_Master
    {
        /*
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
         
         */
    }

    [MetadataType(typeof(ClientMaster_Meta))]
    public partial class Client_Master
    {
    }

    [MetadataType(typeof(PayerMaster_Meta))]
    public partial class Payer_Master
    { }
    [MetadataType(typeof(AddressType_Meta))]
    public partial class Address_Type_Master
    { }

    [MetadataType(typeof(AddressSubType_Meta))]
    public partial class Address_Sub_Type_Master
    { }

    [MetadataType(typeof(AddressMaster_Meta))]
    public partial class Address_Master { }

    [MetadataType(typeof(DispositionMaster_Meta))]
    public partial class Disposition_Master { }

    [MetadataType(typeof(ContactTypeMaster_Meta))]
    public partial class Contact_Type_Master
    {
    }
    [MetadataType(typeof(ContactSubTypeMaster_Meta))]
    public partial class Contact_Sub_Type_Master { }
    [MetadataType(typeof(ContactMaster_Meta))]
    public partial class Contact_Master
    { }
    }
