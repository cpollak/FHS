using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Models
{   
    public class MemberMasterModel
    {
        public int? MemberID { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Primary Phone is required")]
        [Display(Name = "Primary Phone")]
        public string PrimaryPhone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }


        [Display(Name = "Language")]
        public string Language { get; set; }

        [Required(ErrorMessage = "Country Code is required")]
        [Display(Name = "County Code")]
        public string CountyCode { get; set; }

        [Required(ErrorMessage = "ResidentID is required")]
        [Display(Name = "ResidentID")]
        public string ResidentID { get; set; }

        [Required(ErrorMessage = "MedicaidID is required")]
        [Display(Name = "MedicaidID")]
        public string MedicaidID { get; set; }

        [Required(ErrorMessage = "Enrollment Date is required")]
        [Display(Name = "Enrollment Date")]

        public DateTime? EnrollmentDate { get; set; }
        public DateTime? NewEnrollmentDate { get; set; }

        public string EnrollmentDa { get; set; }
        public string NewEnrollmentDa { get; set; }

        [Required(ErrorMessage = "Facility required")]
        [Display(Name = "Facility")]
        public string Facility { get; set; }

        [Required(ErrorMessage = "Disenrolment Date is required")]
        [Display(Name = "Disenrolment Date")]
        public DateTime? DisenrolmentDate { get; set; }

        public string DisenrolmentDa { get; set; }

        [Required(ErrorMessage = "Membership Status is required")]
        [Display(Name = "Membership Status")]
        public int MembershipStatus { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }
        public string Comment { get; set; }
        

        [Required(ErrorMessage = "WorkFlow Status is required")]
        [Display(Name = "WorkFlow Status")]
        public int WorkFlowStatus { get; set; }
        public DateTime? DischargeDate { get; set; }

        

        public string CurrentStatus{ get; set; }

        [Display(Name = "Internal Notes")]
        public string InternalNotes { get; set; }

        [Display(Name = "External Notes")]
        public string ExternalNotes { get; set; }

        [Display(Name = "ChartsID")]
        public string ChartsID { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Temparory Recert Month")]
        public string TempRecertMonth { get; set; }
        
        public string DOB { get; set; }
        public string CreatedUser { get; set; }
        public int? CreatedBy { get; set; }

        public string CreatedDate { get; set; }
        public DateTime? CDate { get; set; }
        public DateTime? FollowUpDate { get; set; }

        public string MembershipStatu { get; set; }
        public string RecertMonth { get; set; }
        public string FollowupCount { get; set; }
        public string Followupdaterange { get; set; }

        public string ReportsType { get; set; }

        public string NextStepTask { get; set; }

        [Display(Name = "Member Status")]
        [Required(ErrorMessage = "Member Status is required")]
        public int MemberStatus { get; set; }

        public string CurrentFacility { get; set; }

        public List<Recertification_Follow_UpModel> Recertification_Follow_UpModels { get; set; }
        public List<ContactModel> contactModels { get; set; }
        public List<SpousalModel> SpousalModels { get; set; }
        public List<LogsModel> LogsModels { get; set; }

        public PITModel pITModel { get; set; }
        public List<PITUploadsModel> PITUploadsModel { get; set; }
        public List<GeneralNotesModel> GeneralNotesModel { get; set; }
    }

    public class ContactModel
    {
        public int ContactID { get; set; }

        public int MemberID { get; set; }

        [Required(ErrorMessage = "RelationShip is required")]
        [Display(Name = "RelationShip")]
        public int RelationShip { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }
    }

    public class SpousalModel
    {
        public int SpousalID { get; set; }

        public int MemberID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Case Name")]
        public string CaseName { get; set; }

        [Display(Name = "MA")]
        public string MA { get; set; }

        
    }

    public class Recertification_Follow_UpModel
    {
        public int FollowUpID { get; set; }
        public int MemberId { get; set; }
        public string CurrentStatus { get; set; }
        public string Outcome { get; set; }
        public string AttemptCount { get; set; }
        public string Notes { get; set; }
        public string NextStepTask { get; set; }
        public string NextStepDueNotes { get; set; }
        public string NewStatus { get; set; }
        public string CallOutcome { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime Nextduedate { get; set; }
    }
    public class LogsModel
    {
        public int? ID { get; set; }

        public int? MemberID { get; set; }

        [Required(ErrorMessage = "Action name is required")]
        [Display(Name = "ActionName")]
        public string ActionName { get; set; }

        [Display(Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }
        public string CreatedDt { get; set; }
       
    }
    public class PITModel
    {
        public int? ID { get; set; }
        public int? MemberID { get; set; }
        public string PITName { get; set; }
        public string PITStatus { get; set; }
        public string PITEffective { get; set; }
        public string PITNotes { get; set; }

    }
    public class GeneralNotesModel
    {
        public int? ID { get; set; }

        public int? MemberId { get; set; }

       
        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedDat { get; set; }

        public int? CreatedBy { get; set; }
        public string CreatedBys { get; set; }
    }

    public class PITUploadsModel
    {
        public int? ID { get; set; }
        public int? MemberID { get; set; }
        public string FileName { get; set; }
       

    }
}
