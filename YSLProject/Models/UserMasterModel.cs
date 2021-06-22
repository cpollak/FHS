using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Models
{
    public class UserMasterModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Enter User Name")]
        [Display(Name = "User Name")]

        public String UserName { get; set; }

        [Required(ErrorMessage = "Enter EmailId")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]

        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }
        public string IsActives { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string LoginDate { get; set; }

        public string UserTypes { get; set; }
        public int UserType { get; set; }
    }
}
