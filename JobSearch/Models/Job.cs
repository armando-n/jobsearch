using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace JobSearch.Models
{
    [Table("Jobs")]
    class Job
    {
        public int JobId { get; set; }
        [MaxLength(100)]
        public string Position { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateApplied { get; set; }
        [MaxLength(100)]
        public string EmploymentService { get; set; }
        public bool? AppliedViaWebsite { get; set; }
        public bool? AppliedViaEmail { get; set; }
        [MaxLength(100)]
        public string StreetAddress { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        public string State { get; set; }
        [RegularExpression(@"^\d{5}$")]
        public int? ZipCode { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int? RecruiterId { get; set; }
        public Recruiter Recruiter { get; set; }

        public List<Job_Communication> Communications { get; set; }
        public List<Job_Interview> Interviews { get; set; }
        public List<Job_Requirement> Requirements { get; set; }
        public List<Job_Responsibility> Responsibilities { get; set; }
        public List<Job_Test> Tests { get; set; }
    }
}
