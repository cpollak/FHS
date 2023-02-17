using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    public class GeneralNotes
    {
        [Key]

        public int? ID { get; set; }

        public int? MemberId { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "FollowUp Date")]
        public DateTime? FollowUpDate { get; set; }

        [Display(Name = "CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        public bool IsDeleted { get; set; }

    }
}
