using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace JobSearch.Models
{
    //[Table("Jobs")]
    public class Job : Template10.Mvvm.BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int JobId { get; set; }
        [MaxLength(100)][NotNull]
        public string Position { get; set; }
        public int? Salary { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateApplied { get; set; }
        [MaxLength(100)]
        public string EmploymentService { get; set; }
        [Default(false, 0)]
        public bool AppliedViaWebsite { get; set; }
        [Default(false, 0)]
        public bool AppliedViaEmail { get; set; }
        [MaxLength(100)]
        public string StreetAddress { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [NotNull]
        public string State { get; set; }
        //[RegularExpression(@"^\d{5}$")]
        public int? ZipCode { get; set; }
        //[MaxLength(100)]
        //public string Area { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }
        //[Default(false, 0)]
        //public bool? EntryLevel { get; set; }
        //[Default(false, 0)]
        //public bool? Inactive { get; set; }

        [ForeignKey(typeof(Company))][NotNull]
        public int CompanyId { get; set; }
        [ManyToOne]
        public Company Company { get; set; }

        [ForeignKey(typeof(Recruiter))]
        public int? RecruiterId { get; set; }
        [ManyToOne]
        public Recruiter Recruiter { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete | CascadeOperation.CascadeRead)]
        public List<Job_Communication> Communications { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete | CascadeOperation.CascadeRead)]
        public List<Job_Interview> Interviews { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete | CascadeOperation.CascadeRead)]
        public List<Job_Requirement> Requirements { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete | CascadeOperation.CascadeRead)]
        public List<Job_Responsibility> Responsibilities { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete | CascadeOperation.CascadeRead)]
        public List<Job_Test> Tests { get; set; }
    }
}
