using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSearch.Models
{
    [Table("Job_Requirements")]
    class Job_Requirement
    {
        public int RequirementId { get; set; }
        [MaxLength(255)]
        public string Requirement { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }
    }
}
