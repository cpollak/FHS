using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    public class PermissionMaster
    {
        [Key]
        public int PermissionID { get; set; }
        public string ControllerName { get; set; }
        public string FunctionName { get; set; }
        public string URL { get; set; }
        public string PermissionName { get; set; }
        public int UserType { get; set; }
        public int UserId { get; set; }

        public int CreatedId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
