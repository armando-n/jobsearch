using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace JobSearch.Models
{
    [Table("Job_Interviews")]
    class Job_Interview
    {
        public int InterviewId { get; set; }
        public string Via { get; set; }
        public DateTime DateAndTime { get; set; }
        [MaxLength(100)]
        public string Interviewer { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }
    }
}
