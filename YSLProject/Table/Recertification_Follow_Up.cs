using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    public class Recertification_Follow_Up
    {
        [Key]
        public int FollowUpID { get; set; }
        public int MemberId { get; set; }

        [Display(Name = "Current Status")]
        public string CurrentStatus { get; set; }
        public string Outcome { get; set; }
        public string AttemptCount { get; set; }
        public string Notes { get; set; }

        [Display(Name = "Next Step Task")]
        public string NextStepTask { get; set; }

        [Display(Name = "Next Step Due Notes")]
        public string NextStepDueNotes { get; set; }

        [Display(Name = "New Status")]
        public string NewStatus { get; set; }
        public string CallOutcome { get; set; }

        [Display(Name = "Period Of The Day")]
        public string PeriodOfTheDay { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }
        public DateTime Nextduedate { get; set; }
    }
}
