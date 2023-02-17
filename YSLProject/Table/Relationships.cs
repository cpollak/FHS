using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Table
{
    [Table("Relationship")]
    public class Relationships
    {
        [Key]
        public int RelationshipID { get; set; }
        public string Relationship { get; set; }
    }
}
