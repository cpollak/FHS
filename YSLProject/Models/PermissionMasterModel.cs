using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Models
{
    public class PermissionMasterModel
    {
        public int PermissionID { get; set; }
        public string ControllerName { get; set; }
        public string FunctionName { get; set; }
        public string URL { get; set; }
        public string PermissionName { get; set; }
        public int UserType { get; set; }
        public int UserId { get; set; }

        public int CreatedId { get; set; }
        public string CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
