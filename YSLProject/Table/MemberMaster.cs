using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    public class MemberMaster
    {
        [Key]

        public int MemberID { get; set; }

       // [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

      //  [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

      //  [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

      //  [Required(ErrorMessage = "Primary Phone is required")]
        [Display(Name = "Primary Phone")]
        public string PrimaryPhone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        
        [Display(Name = "Language")]
        public string Language { get; set; }

      //  [Required(ErrorMessage = "Country Code is required")]
        [Display(Name = "County Code")]
        public string CountyCode { get; set; }

     //   [Required(ErrorMessage = "ResidentID is required")]
        [Display(Name = "ResidentID")]
        public string ResidentID { get; set; }

     //   [Required(ErrorMessage = "MedicaidID is required")]
        [Display(Name = "MedicaidID")]
        public string MedicaidID { get; set; }

        [Display(Name = "ChartsID")]
        public string ChartsID { get; set; }

        //   [Required(ErrorMessage = "Enrollment Date is required")]
        [Display(Name = "Enrollment Date")]
        public DateTime? EnrollmentDate { get; set; }
        [Display(Name = "New Enrollment Date")]
        public DateTime? NewEnrollmentDate { get; set; }


        
        //   [Required(ErrorMessage = "Disenrolment Date is required")]
        [Display(Name = "Disenrolment Date")]
        public DateTime? DisenrolmentDate { get; set; }

     //   [Required(ErrorMessage = "Membership Status is required")]
        [Display(Name = "Membership Status")]
        public int? MembershipStatus { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

     //   [Required(ErrorMessage = "WorkFlow Status is required")]
        [Display(Name = "WorkFlow Status")]
        public int? WorkFlowStatus { get; set; }

        [Display(Name = "Internal Notes")]
        public string InternalNotes { get; set; }

        [Display(Name = "External Notes")]
        public string ExternalNotes { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Follow Up Date")]
        public DateTime? FollowUpDate { get; set; }

        [Display(Name = "Temparory Recert Month")]
        public string TempRecertMonth { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DischargeDate { get; set; }


        
        public int? CreatedBy { get; set; }

        public string CIN { get; set; }

        public string RecertMonth { get; set; }

        public string Comment { get; set; }

        public string Coverage { get; set; }

        public string Payer { get; set; }

        public string PlanCode { get; set; }

        public string MedicarePayer { get; set; }

        public string CarrierCode { get; set; }

        public string ExceptionCodes { get; set; }

        public int? Status { get; set; }

        public string Gender { get; set; }

        public string Facility { get; set; }
    }
}
