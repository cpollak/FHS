using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    public class UserMaster
    {
        [Key]

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

        public DateTime LastLoginDate { get; set; }

        [NotMapped]
        public string LoginDate { get; set; }

        [NotMapped]
        public DateTime CreatedDate { get; set; }

        public int UserType { get; set; }

        [NotMapped]
        public string UserTypes { get; set; }
    }
}
