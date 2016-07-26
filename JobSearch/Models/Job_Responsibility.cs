using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace JobSearch.Models
{
    [Table("Job_Responsibilities")]
    class Job_Responsibility
    {
        public int ResponsibilityId { get; set; }
        [MaxLength(255)]
        public string Responsibility { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }
    }
}
