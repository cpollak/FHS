using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    public class Logs
    {
        [Key]

        public int ID { get; set; }

        public int MemberID { get; set; }       

        [Display(Name = "ActionName")]
        public string ActionName { get; set; }               

        [Display(Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        //public string Facility { get; set; }

    }
}
