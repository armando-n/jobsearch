using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;

namespace JobSearch.Models
{
    public class Job : Template10.Mvvm.BindableBase
    {
        public enum JobState { Applied, Interested, WaitingOnMeToCommunicate, WaitingOnJobToCommunicate, UpcomingTest, UpcomingInterview, DeclinedToOffer, Offered, OfferDeclined }

        [PrimaryKey, AutoIncrement]
        public int JobId { get; set; }
        [MaxLength(100)][NotNull]
        public string Position { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        [Default(false, 0)]
        public bool IsSalaryEstimate { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateApplied { get; set; }
        [MaxLength(100)]
        public string EmploymentService { get; set; }
        [MaxLength(1000)]
        public string EmploymentServiceJobLink { get; set; }
        [Default(false, 0)]
        public bool AppliedViaService { get; set; }
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
        public int? ZipCode { get; set; }
        [MaxLength(100)][NotNull]
        public string Area { get; set; }
        
        private string _notes;
        [MaxLength(1000)]
        public string Notes
        {
            get { return _notes; }
            set
            {
                Set(ref _notes, value);
                RaisePropertyChanged(Notes);
            }
        }
        public int? YearsExperienceNeeded { get; set; }
        [Default(false, 1)]
        public bool Active { get; set; }

        private bool _flagged;
        [Default(false, 0)]
        public bool Flagged
        {
            get { return _flagged; }
            set { Set(ref _flagged, value); }
        }

        public JobState StateOfJob { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string StateColor
        {
            get
            {
                switch (this.StateOfJob)
                {
                    case JobState.Applied: return "DodgerBlue";
                    case JobState.Interested: return "White";
                    case JobState.WaitingOnMeToCommunicate: return "DarkRed";
                    case JobState.WaitingOnJobToCommunicate: return "DarkOrange";
                    case JobState.UpcomingTest: return "Gold";
                    case JobState.UpcomingInterview: return "Gold";
                    case JobState.DeclinedToOffer: return "SaddleBrown";
                    case JobState.Offered: return "Green";
                    case JobState.OfferDeclined: return "SlateBlue";
                    default: return "White";
                }
            }
        }

        [ForeignKey(typeof(Company))][NotNull]
        public int CompanyId { get; set; }
        [ManyToOne]
        public Company Company { get; set; }

        [ForeignKey(typeof(Recruiter))]
        public int? RecruiterId { get; set; }
        [ManyToOne]
        public Recruiter Recruiter { get; set; }

        [Default(false, 0)]
        public bool Stale { get; set; }
        [Default(false, 0)]
        public bool Archived { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete | CascadeOperation.CascadeRead)]
        public ObservableCollection<Job_Communication> Communications { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete | CascadeOperation.CascadeRead)]
        public ObservableCollection<Job_Interview> Interviews { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete | CascadeOperation.CascadeRead)]
        public ObservableCollection<Job_Requirement> Requirements { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete | CascadeOperation.CascadeRead)]
        public ObservableCollection<Job_Responsibility> Responsibilities { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete | CascadeOperation.CascadeRead)]
        public ObservableCollection<Job_Test> Tests { get; set; }
    }
}
