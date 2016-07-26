using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace JobSearch.Models
{
    [Table("Job_Communications")]
    class Job_Communication
    {
        public int CommunicationId { get; set; }
        [MaxLength(100)]
        public string Via { get; set; }
        [MaxLength(100)]
        public string To { get; set; }
        [MaxLength(100)]
        public string From { get; set; }
        [MaxLength(100)]
        public string Subject { get; set; }
        [MaxLength(5000)]
        public string Description { get; set; }
        public DateTime DateAndTime { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }
    }
}
