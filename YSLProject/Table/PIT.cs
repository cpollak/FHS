using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    public class PIT
    {
        [Key]

        public int? ID { get; set; }

        public int? MemberID { get; set; }

        [Display(Name = "PIT")]
        public string PITName { get; set; }

        [Display(Name = "PITStatus")]
        public string PITStatus { get; set; }

        [Display(Name = "PITEffective")]
        public string PITEffective { get; set; }

        [Display(Name = "PITNotes")]
        public string PITNotes { get; set; }

       
    }
}
