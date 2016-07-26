using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace JobSearch.Models
{
    [Table("Job_Test")]
    class Job_Test
    {
        public int TestId { get; set; }
        public string Type { get; set; } // enum('online', 'in-person')
        public DateTime DateAndTime { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }
    }
}
