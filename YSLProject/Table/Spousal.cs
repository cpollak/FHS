using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    public class Spousal
    {
        [Key]

        public int SpousalID { get; set; }

        public int MemberID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Case Name")]
        public string CaseName { get; set; }

        [Display(Name = "MA")]
        public string MA { get; set; }

       
       
    }
}
