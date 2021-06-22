using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    public class PITUploads
    {
        [Key]

        public int? ID { get; set; }

        public int? MemberID { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

    }
}
