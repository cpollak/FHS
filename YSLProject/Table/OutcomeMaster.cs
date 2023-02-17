using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{

    public class OutcomeMaster
    {
        [Key]
        public int Id { get; set; }
        public int CurrentStatusId { get; set; }
        public string Name { get; set; }
        public string NextStepTask { get; set; }
        public string NewStatus { get; set; }
        public int? NextDueHours { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool NotesRequired { get; set; }
    }

    public class CurrentStatusMaster
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
