using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Models
{
    public class RecertificationFollowUp
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
        public string PeriodOfTheDay { get; set; }

        public int CreatedBy { get; set; }



        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Nextduedate { get; set; }


        public List<RecertificationFollowUpList> recertificLists { get; set; }

    }


    public class RecertificationFollowUpList
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
        public int CreatedBy { get; set; }

        public string Nextduedate { get; set; }
    }
}
