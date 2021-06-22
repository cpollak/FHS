using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    public class LanguageMaster
    {
        [Key]
        public int LanguageID { get; set; }
        public string Language { get; set; }
    }
}
