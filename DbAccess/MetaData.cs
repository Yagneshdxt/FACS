using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess
{

    public class ClientGroupMaster_Meta
    {
        [Display(Name = "Hospital Group Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Hospital Group Name")]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Hospital_Group_Name { get; set; }

        [Display(Name = "Hospital Group Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Hospital Group Code")]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Hospital_Group_Code { get; set; }

        [Display(Name = "Commission Percentage")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Commission Percentage")]
        public decimal Commission_Percentage { get; set; }

    }

    public class Payment_Meta {
        [Display(Name = "Patient")]
        public int Patient_Id { get; set; }

        [Display(Name = "Payment Index")]
        [Required(ErrorMessage = "Please enter Payment Index")]
        public int? Payment_Index { get; set; }


        [Display(Name = "Payment Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Payment Type")]
        public string Payment_Type { get; set; }

        [Display(Name = "Payment Amount")]
        [Required(ErrorMessage = "Please enter Payment Amount")]
        public decimal? Payment_Amount { get; set; }


        [Display(Name = "Payment Date")]
        [Required(ErrorMessage = "Please enter Payment Date")]
        public Nullable<System.DateTime> Payment_Date { get; set; }

        [Display(Name = "Payment Post Date")]
        [Required(ErrorMessage = "Please enter Payment Post Date")]
        public Nullable<System.DateTime> Payment_Post_Date { get; set; }

        [Display(Name = "Payment Details")]
        public string Payment_Details { get; set; }
        
        public bool IsActive { get; set; }

        [Display(Name = "Payment Revenue")]
        [Required(ErrorMessage = "Please enter Revenue")]
        public decimal? Revenue { get; set; }
    }

    public class PatientStatus_Meta {

        [Display(Name = "Patient")]
        public int Patient_Id { get; set; }

        [Display(Name = "Collector")]
        public int Collector_Id { get; set; }

        [Display(Name = "Disposition")]
        public int Disposition_Id { get; set; }

        [Display(Name = "Contact Date")]
        [Required(ErrorMessage = "Please enter Treatment Date")]
        public Nullable<System.DateTime> Contact_Date { get; set; }

        [Display(Name = "Method Of Contact")]
        public string Method_Of_Contact { get; set; }

        [Display(Name = "Valid Contact Number")]
        public string Valid_Contact_Number { get; set; }

        [Display(Name = "Valid Contact Number Type")]
        public string Valid_Contact_Number_Type { get; set; }

        [Display(Name = "Current Balance")]
        [Required(ErrorMessage = "Please enter Current Balance")]
        public decimal? Current_Balance { get; set; }

        public string Notes { get; set; }

        [Display(Name = "Is Latest?")]
        public bool IsLatest { get; set; }
        public Nullable<int> Created_By_User { get; set; }
        public Nullable<int> Updated_By_User { get; set; }
        public Nullable<System.DateTime> Create_Dt_Time { get; set; }
        public Nullable<System.DateTime> Uppdate_Dt_Time { get; set; }

        [Display(Name = "Last Payment Date")]
        public Nullable<System.DateTime> Last_Payment_Date { get; set; }

        [Display(Name = "Last Payment Amount")]
        public Nullable<decimal> Last_Payment_Amount { get; set; }

        [Display(Name = "Total Paid Amount")]
        public Nullable<decimal> Total_Paid_Amount { get; set; }
    }

    public class PatientTreatments_Meta
    {
        [Display(Name = "Patient")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select Patient")]
        public int Patient_Id { get; set; }

        [Display(Name = "Treatment Date")]
        [Required(ErrorMessage = "Please enter Treatment Date")]
        public System.DateTime Treatment_Date { get; set; }

        [Display(Name = "Treatment Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Treatment Code")]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long upto 10 characters", MinimumLength = 2)]
        public string Treatment_Code { get; set; }

        [Display(Name = "Treatment Description")]
        public string Treatment_Description { get; set; }

        [Display(Name = "Total Charge")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Total Charge")]
        public decimal Total_Charges { get; set; }
    }

    public class PatientTypeMaster_Meta
    {
        [Display(Name = "Patient Type Code", Description = "Patient Type Code")]

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Patient Type Code")]
        [StringLength(2, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Patient_Type_Code { get; set; }

        [Display(Name = "Patient Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Patient Type")]
        public string Patient_Type { get; set; }

    }

    public class PatientReceivablesInfo_Meta
    {

        [Display(Name = "Patient")]
        public int Patient_Id { get; set; }

        [Display(Name = "Patient Type Code")]
        public string Patient_Type_Code { get; set; }

        [Display(Name = "Payer")]
        public int Payer_Id { get; set; }
    }

    public class PatientMaster_Meta
    {
        [Display(Name = "Patient No. From Client")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Patient No.")]
        public string PatientNoFromClient { get; set; }

        [Display(Name = "Patient Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Patient Last Name")]
        public string Patient_Last_Name { get; set; }

        [Display(Name = "Patient First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Patient First Name")]
        public string Patient_First_Name { get; set; }

        [Display(Name = "Patient Middle Name")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Patient Middle Name")]
        public string Patient_Middle_Name { get; set; }

        [Display(Name = "Patient Social Security No.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Social Security No.")]
        public string Patient_SocialSecurity { get; set; }

        [Display(Name = "Hospital")]
        [Range(0, int.MaxValue, ErrorMessage = "Please Select Hospital")]
        [Required(ErrorMessage = "Please Select Hospital")]
        public int? Hospital_Id { get; set; }

        [Display(Name = "Admit Date")]
        [Required(ErrorMessage = "Please enter Admit Date")]
        public Nullable<System.DateTime> Admit_Date { get; set; }

        [Display(Name = "Discharge Date")]
        [Required(ErrorMessage = "Please enter Discharge Date")]
        public Nullable<System.DateTime> Discharge_Date { get; set; }

        [Display(Name = "Bill Amount")]
        [Required(ErrorMessage = "Please enter Bill Amount")]
        public decimal? Patient_Bill_Amount { get; set; }

        [Display(Name = "Insurance Bill Amount")]
        [Required(ErrorMessage = "Please enter Insurance Bill Amount")]
        public decimal? Patient_Insurance_Bill_Amount { get; set; }

        [Display(Name = "Total Charge")]
        [Required(ErrorMessage = "Please enter Total Charges")]
        public decimal? Total_Charges { get; set; }


        [Display(Name = "Placement Date")]
        [Required(ErrorMessage = "Please enter Placement Date")]
        public Nullable<System.DateTime> Placement_Date { get; set; }

        public bool IsActive { get; set; }
        public bool IsClosed { get; set; }

        [Display(Name = "Closed Date")]
        [Required(ErrorMessage = "Please enter Closed Date")]
        public Nullable<System.DateTime> Closed_Date { get; set; }
    }

    public class AddressMaster_Meta
    {

        [Display(Name = "Address Type")]
        public int Address_Type { get; set; }

        [Display(Name = "Address Sub Type")]
        public int Address_Sub_Type { get; set; }

        [Display(Name = "Address Line 1")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Address Line 1")]
        public string Address_Line_1 { get; set; }

        [Display(Name = "Address Line 2")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Address Line 2")]
        public string Address_Line_2 { get; set; }

        [Display(Name = "Address Line 3")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Address Line 3")]
        public string Address_Line_3 { get; set; }

        [Display(Name = "City")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter City")]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter State")]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Zip Code")]
        public string ZipCode { get; set; }

    }

    public class DispositionMaster_Meta
    {
        [Display(Name = "Disposition")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Disposition")]
        public string Disposition { get; set; }

        [Display(Name = "Disposition Group")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Disposition Group")]
        public string Disposition_Group { get; set; }


        [Display(Name = "Is Active")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Is Active")]
        public bool IsActive { get; set; }
    }

    public class AddressSubType_Meta
    {
        [Display(Name = "Address Type")]
        [Required(ErrorMessage = "Please Select Address Type")]
        public int? Address_Type_Id { get; set; }

        [Display(Name = "Address Sub Type")]
        [Required(ErrorMessage = "Please enter Address Sub Type", AllowEmptyStrings = false)]
        public string Address_Sub_Type { get; set; }

    }

    public class AddressType_Meta
    {
        [Display(Name = "Address Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Address Type")]
        public string Address_Type { get; set; }
    }

    public class PayerMaster_Meta
    {
        [Display(Name = "Payer Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Payer Name")]
        public string Payer_Name { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }

    public class ContactMaster_Meta
    {
        [Display(Name = "Contact Type")]
        [Required(ErrorMessage = "Please Select Contact Type")]
        public int? Contact_Type { get; set; }

        [Display(Name = "Contact Sub Type")]
        [Required(ErrorMessage = "Please Select Contact Sub Type")]
        public int? Contact_Sub_Type { get; set; }

        [Display(Name = "Country Code")]
        public string Country_Code { get; set; }

        [Display(Name = "City Code")]
        public string City_Code { get; set; }

        [Display(Name = "Phone 1")]
        public string Phone_1 { get; set; }

        [Display(Name = "Phone 2")]
        public string Phone_2 { get; set; }

        [Display(Name = "Phone 3")]
        public string Phone_3 { get; set; }

        [Display(Name = "Phone 4")]
        public string Phone_4 { get; set; }

        [Display(Name = "Phone 5")]
        public string Phone_5 { get; set; }

        [Display(Name = "Cell 1")]
        public string Cell_1 { get; set; }

        [Display(Name = "Cell 2")]
        public string Cell_2 { get; set; }

        [Display(Name = "Fax 1")]
        public string Fax_1 { get; set; }

        [Display(Name = "Fax 2")]
        public string Fax_2 { get; set; }

        [Display(Name = "Email 1")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email_1 { get; set; }

        [Display(Name = "Email 2")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email_2 { get; set; }

        [Display(Name = "Contact Person Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Contact Person Name")]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Contact_Person_Name { get; set; }

    }

    public class ContactSubTypeMaster_Meta
    {

        [Display(Name = "Contact Type")]
        [Required(ErrorMessage = "Please Selec Contact Type")]
        public int Contact_Type_Id { get; set; }

        [Display(Name = "Contact Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Contact Type name")]
        public string Contact_Sub_Type { get; set; }



    }

    public class ContactTypeMaster_Meta
    {
        [Display(Name = "Contact Type Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Contact Type Name")]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Contact_Type { get; set; }
        [Display(Name = "Created Date")]
        public Nullable<System.DateTime> Create_Dt_Time { get; set; }
        [Display(Name = "Updated Date")]
        public Nullable<System.DateTime> Update_Dt_Time { get; set; }
    }

    public class ClientMaster_Meta
    {

        [Display(Name = "Hospital Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Hospital Name")]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Hospital_Name { get; set; }

        [Display(Name = "Hospital Group")]
        public Nullable<int> Hospital_Group_Id { get; set; }

        [Display(Name = "Hospital Speciality")]
        public string Hospital_Speciality { get; set; }

        [Display(Name = "Number Of Beds")]
        public Nullable<int> Number_Of_Beds { get; set; }

        [Display(Name = "ICCU Available")]
        public Nullable<bool> ICCU_Available { get; set; }

        [Display(Name = "Visitor Hours")]
        public string Visitor_Hours { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }
    }

}
