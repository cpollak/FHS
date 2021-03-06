using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    public class MemberMasterSP
    {
        [Key]
        public int? MemberID { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Facility { get; set; }
        public string MedicaidID { get; set; }
        public string? ResidentID { get; set; }
        public int? MembershipStatus { get; set; }
        public string Language { get; set; }
        public string RecertMonth { get; set; }

        public int? CreatedBy { get; set; }
        public string PrimaryPhone { get; set; }

        public int? AssignId { get; set; }
    }
}
