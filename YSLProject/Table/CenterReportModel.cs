using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    public class CenterReportModel
    {
        [Key]
        public int MemberID { get; set; }
        public string Plan { get; set; }
        public string County { get; set; }
        public string MemberCIN { get; set; }
        public string Member { get; set; }
        public string CurrentStatus { get; set; }
        public string NextStepTask { get; set; }
        public string MedicalExpDate { get; set; }
        public string RecertDueDate { get; set; }
        public string DateLettSent { get; set; }
        public string DateofFirstcallAttm { get; set; }
        public string DateofFirstSuces { get; set; }
        public string DateofFinalAttm { get; set; }
        public string CountofOut { get; set; }
        public string DatesubReceCon { get; set; }
        public string SubmittedbyFhs { get; set; }
        public string ReasonFhs { get; set; }
        public string SubmittedbyHra { get; set; }
        public string ReasonSubAfter { get; set; }
        public string DidMemberLos { get; set; }
        public string DateofFiout { get; set; }
        public string ReasonMediClose { get; set; }

        public string Comment { get; set; }
        public string Comment2 { get; set; }
        [NotMapped]
        public string Comment3 { get; set; }
        [NotMapped]
        public string Comment4 { get; set; }
        [NotMapped]
        public string Comment5 { get; set; }
        [NotMapped]
        public string Comment6 { get; set; }
        [NotMapped]
        public string Comment7 { get; set; }
        [NotMapped]
        public string Comment8 { get; set; }
        [NotMapped]
        public string Comment9 { get; set; }
        [NotMapped]
        public string Comment10 { get; set; }
        [NotMapped]
        public string RecertDate { get; set; }
    }

    public class CenterReportModelNon
    {
        [Key]
        public int MemberID { get; set; }
        public string Plan { get; set; }
        public string County { get; set; }
        public string MemberCIN { get; set; }
        public string Member { get; set; }
        public string CurrentStatus { get; set; }
        public string NextStepTask { get; set; }
        public string MedicalExpDate { get; set; }
        public string RecertDueDate { get; set; }
        public string DateLettSent { get; set; }
        public string DateofFirstcallAttm { get; set; }
        public string DateofFirstSuces { get; set; }
        public string DateofFinalAttm { get; set; }
        public string CountofOut { get; set; }
        public string DatesubReceCon { get; set; }
        public string SubmittedbyFhs { get; set; }
        public string ReasonFhs { get; set; }
        public string SubmittedbyHra { get; set; }
        public string ReasonSubAfter { get; set; }
        public string DidMemberLos { get; set; }
        public string DateofFiout { get; set; }
        public string ReasonMediClose { get; set; }

        public string Comment { get; set; }
        public string Comment2 { get; set; }
        [NotMapped]
        public string Comment3 { get; set; }
        [NotMapped]
        public string Comment4 { get; set; }
        [NotMapped]
        public string Comment5 { get; set; }
        [NotMapped]
        public string Comment6 { get; set; }
        [NotMapped]
        public string Comment7 { get; set; }
        [NotMapped]
        public string Comment8 { get; set; }
        [NotMapped]
        public string Comment9 { get; set; }
        [NotMapped]
        public string Comment10 { get; set; }
        
        public string RecertDate { get; set; }
    }
    public class ReportNonCExcelExportModel
    {
        [Key]
        public int MemberID { get; set; }
        public string Plan { get; set; }
        public string County { get; set; }
        public string MemberCIN { get; set; }
        public string Member { get; set; }
        public string CurrentStatus { get; set; }
        public string NextStepTask { get; set; }
        //public string MedicalExpDate { get; set; }
        //public string RecertDueDate { get; set; }
        //public string DateLettSent { get; set; }
        //public string DateofFirstcallAttm { get; set; }
        public string DatesubReceCon { get; set; }
        //public string ReasonSubAfter { get; set; }
        public string Comment { get; set; }
        public string Comment2 { get; set; }
        [NotMapped]
        public string Comment3 { get; set; }
        [NotMapped]
        public string Comment4 { get; set; }
        [NotMapped]
        public string Comment5 { get; set; }
        [NotMapped]
        public string Comment6 { get; set; }
        [NotMapped]
        public string Comment7 { get; set; }
        [NotMapped]
        public string Comment8 { get; set; }
        [NotMapped]
        public string Comment9 { get; set; }
        [NotMapped]
        public string Comment10 { get; set; }
    }
    public class ReportExcelExportModel
    {
        [Key]
        public int MemberID { get; set; }
        public string Plan { get; set; }
        public string County { get; set; }
        public string MemberCIN { get; set; }
        public string Member { get; set; }
        public string CurrentStatus { get; set; }
        public string NextStepTask { get; set; }
        public string MedicalExpDate { get; set; }
        public string RecertDueDate { get; set; }
        public string DateLettSent { get; set; }
        public string DateofFirstcallAttm { get; set; }
        public string DatesubReceCon { get; set; }
        public string ReasonSubAfter { get; set; }
        public string Comment { get; set; }
        public string Comment2 { get; set; }
        [NotMapped]
        public string Comment3 { get; set; }
        [NotMapped]
        public string Comment4 { get; set; }
        [NotMapped]
        public string Comment5 { get; set; }
        [NotMapped]
        public string Comment6 { get; set; }
        [NotMapped]
        public string Comment7 { get; set; }
        [NotMapped]
        public string Comment8 { get; set; }
        [NotMapped]
        public string Comment9 { get; set; }
        [NotMapped]
        public string Comment10 { get; set; }
    }
}
