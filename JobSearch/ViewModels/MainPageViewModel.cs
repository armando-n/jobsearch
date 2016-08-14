using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Services.NavigationService;
using Template10.Mvvm;
using JobSearch.Models;

namespace JobSearch.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        Services.DatabaseService.DatabaseService db;

        public MainPageViewModel()
        {
            _instance = this;
            db = Services.DatabaseService.DatabaseService.GetDB();
        }

        private static MainPageViewModel _instance;
        public static MainPageViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainPageViewModel();
                return _instance;
            }
            set { _instance = value; }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            //if (suspensionState.Any())
            //{
            //    Value = suspensionState[nameof(Value)]?.ToString();
            //}
            //await Task.CompletedTask;

            Jobs = new ObservableCollection<Job>(db.Jobs);
            Selected = (Jobs.Count > 0) ? Jobs.First() : null;
            Requirements = new ObservableCollection<Job_Requirement>(db.Requirements);
            Responsibilities = new ObservableCollection<Job_Responsibility>(db.Responsibilities);
            Tests = new ObservableCollection<Job_Test>(db.Tests);
            Interviews = new ObservableCollection<Job_Interview>(db.Interviews);
            Communications = new ObservableCollection<Job_Communication>(db.Communications);

            return Task.CompletedTask;
        }

        //public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        //{
        //    if (suspending)
        //    {
        //        suspensionState[nameof(Value)] = Value;
        //    }
        //    await Task.CompletedTask;
        //}

        private ObservableCollection<Job> _jobs;
        public ObservableCollection<Job> Jobs
        {
            get { return _jobs; }
            set { Set(ref _jobs, value); }
        }

        private ObservableCollection<Job_Requirement> _requirements;
        public ObservableCollection<Job_Requirement> Requirements
        {
            get { return _requirements; }
            set { Set(ref _requirements, value);  }
        }

        private ObservableCollection<Job_Responsibility> _responsiblities;
        public ObservableCollection<Job_Responsibility> Responsibilities
        {
            get { return _responsiblities; }
            set { Set(ref _responsiblities, value); }
        }

        private ObservableCollection<Job_Test> _tests;
        public ObservableCollection<Job_Test> Tests
        {
            get { return _tests; }
            set { Set(ref _tests, value); }
        }

        private ObservableCollection<Job_Interview> _interviews;
        public ObservableCollection<Job_Interview> Interviews
        {
            get { return _interviews; }
            set { Set(ref _interviews, value); }
        }

        private ObservableCollection<Job_Communication> _communications;
        public ObservableCollection<Job_Communication> Communications
        {
            get { return _communications; }
            set { Set(ref _communications, value); }
        }

        private Job _selected = default(Job);
        public object Selected
        {
            get { return _selected; }
            set
            {
                var job = value as Job;
                Set(ref _selected, job);
            }
        }

        public void AddJob(string position, int? minSalary, int? maxSalary, string company, string recruiter, string notes
                , int? yearsExperienceNeeded, string employmentService, string employmentServiceJobLink
                , bool? appliedViaService, bool? appliedViaWebsite, bool? appliedViaEmail, DateTime? datePosted, DateTime? dateApplied
                , string streetAddress, string city, string state, int? zipCode, string area, bool? active, bool? flagged)
        {
            try
            {
                Job newJob = new Job()
                {
                    Position = String.IsNullOrWhiteSpace(position) ? null : position,
                    MinSalary = minSalary,
                    MaxSalary = maxSalary,
                    YearsExperienceNeeded = yearsExperienceNeeded,

                    Notes = String.IsNullOrWhiteSpace(notes) ? null : notes,
                    EmploymentService = String.IsNullOrWhiteSpace(employmentService) ? null : employmentService,
                    EmploymentServiceJobLink = String.IsNullOrWhiteSpace(employmentServiceJobLink) ? null : employmentServiceJobLink,
                    AppliedViaService = appliedViaService ?? false,
                    AppliedViaWebsite = appliedViaWebsite ?? false,
                    AppliedViaEmail = appliedViaEmail ?? false,
                    DatePosted = datePosted,
                    DateApplied = dateApplied,
                    StreetAddress = String.IsNullOrWhiteSpace(streetAddress) ? null : streetAddress,
                    City = String.IsNullOrWhiteSpace(city) ? null : city,
                    State = String.IsNullOrWhiteSpace(state) ? null : state,
                    ZipCode = zipCode,
                    Area = String.IsNullOrWhiteSpace(area) ? null : area,
                    Active = active ?? true,
                    Flagged = flagged ?? false,
                    Communications = new ObservableCollection<Job_Communication>(),
                    Interviews = new ObservableCollection<Job_Interview>(),
                    Requirements = new ObservableCollection<Job_Requirement>(),
                    Responsibilities = new ObservableCollection<Job_Responsibility>(),
                    Tests = new ObservableCollection<Models.Job_Test>()
                };

                db.AddJob(newJob, company, recruiter);
                Jobs.Add(newJob);
                RaisePropertyChanged(nameof(Jobs));
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex)
            {
                throw new ArgumentNullException("Required fields were omitted for the new job");
            }
        }

        public void AddRequirement(string requirement)
        {
            try
            {
                Job currentJob = Selected as Job;

                Job_Requirement newRequirement = new Job_Requirement()
                {
                    Requirement = String.IsNullOrWhiteSpace(requirement) ? null : requirement
                };

                db.AddJobRequirement(newRequirement, currentJob.JobId);
                Requirements.Add(newRequirement);
                RaisePropertyChanged(nameof(Jobs));
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex)
            {
                throw new ArgumentNullException("Required fields were omitted for the new job requirement. " + ex.Message);
            }
        }

        public void EditRequirement(string newRequirement, int requirementId)
        {
            Job currentJob = Selected as Job;
            Job_Requirement jobReq = currentJob.Requirements.Where(rqrmt => rqrmt.RequirementId == requirementId).Single();
            jobReq.Requirement = String.IsNullOrWhiteSpace(newRequirement) ? null : newRequirement;

            db.Update(jobReq);
        }

        public void DeleteRequirement(int requirementId)
        {
            Job currentJob = Selected as Job;
            Job_Requirement jobReq = currentJob.Requirements.Where(rqrmt => rqrmt.RequirementId == requirementId).Single();

            db.DeleteJobRequirement(jobReq, currentJob.JobId);
            Requirements.Remove(jobReq);
        }

        public void AddResponsibility(string responsibility)
        {
            try
            {
                Job currentJob = Selected as Job;

                Job_Responsibility newResponsibility = new Job_Responsibility()
                {
                    Responsibility = String.IsNullOrWhiteSpace(responsibility) ? null : responsibility
                };

                db.AddJobResponsibility(newResponsibility, currentJob.JobId);
                Responsibilities.Add(newResponsibility);
                RaisePropertyChanged(nameof(Jobs));
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex)
            {
                throw new ArgumentNullException("Required fields were omitted for the new job requirement. " + ex.Message);
            }
        }

        public void EditResponsibility(string newResponsibility, int responsibilityId)
        {
            Job currentJob = Selected as Job;
            Job_Responsibility jobResp = currentJob.Responsibilities.Where(resp => resp.ResponsibilityId == responsibilityId).Single();
            jobResp.Responsibility = String.IsNullOrWhiteSpace(newResponsibility) ? null : newResponsibility;

            db.Update(jobResp);
        }

        public void DeleteResponsibility(int responsibilityId)
        {
            Job currentJob = Selected as Job;
            Job_Responsibility jobResp = currentJob.Responsibilities.Where(resp => resp.ResponsibilityId == responsibilityId).Single();

            db.DeleteJobResponsibility(jobResp, currentJob.JobId);
            Responsibilities.Remove(jobResp);
        }

        public void AddTest(string type, DateTime? date, TimeSpan time, string notes)
        {
            try
            {
                Job currentJob = Selected as Job;

                Job_Test newTest = new Job_Test()
                {
                    Type = String.IsNullOrWhiteSpace(type) ? null : type,
                    DateAndTime = date?.Add(time),
                    Notes = String.IsNullOrWhiteSpace(notes) ? null : notes
                };

                db.AddJobTest(newTest, currentJob.JobId);
                Tests.Add(newTest);
                RaisePropertyChanged(nameof(Jobs));
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex)
            {
                throw new ArgumentNullException("Required fields were omitted for the new test");
            }
        }

        public void AddInterview(string via, string interviewer, DateTime? date, TimeSpan time, string notes)
        {
            try
            {
                Job currentJob = Selected as Job;

                Job_Interview newInterview = new Job_Interview()
                {
                    Via = String.IsNullOrWhiteSpace(via) ? null : via,
                    Interviewer = String.IsNullOrWhiteSpace(interviewer) ? null : interviewer,
                    DateAndTime = date?.Add(time),
                    Notes = String.IsNullOrWhiteSpace(notes) ? null : notes
                };

                db.AddJobInterview(newInterview, currentJob.JobId);
                Interviews.Add(newInterview);
                RaisePropertyChanged(nameof(Jobs));
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex)
            {
                throw new ArgumentNullException("Required fields were omitted for the new interview");
            }
        }

        public void AddCommunication(string to, string from, string subject, string via, DateTime? date, TimeSpan time, string description)
        {
            try
            {
                Job currentJob = Selected as Job;

                Job_Communication newCommunication = new Job_Communication()
                {
                    To = String.IsNullOrWhiteSpace(to) ? null : to,
                    From = String.IsNullOrWhiteSpace(from) ? null : from,
                    Subject = String.IsNullOrWhiteSpace(subject) ? null : subject,
                    Via = String.IsNullOrWhiteSpace(via) ? null : via,
                    DateAndTime = date?.Add(time),
                    Description = String.IsNullOrWhiteSpace(description) ? null : description
                };

                db.AddJobCommunication(newCommunication, currentJob.JobId);
                Communications.Add(newCommunication);
                RaisePropertyChanged(nameof(Jobs));
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex)
            {
                throw new ArgumentNullException("Required fields were omitted for the new communication");
            }
        }

        public void SetNotes(string notes)
        {
            try {
                db.SetJobNotes(notes, (Selected as Job).JobId);
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex) {
                throw new ArgumentNullException("Required fields were omitted for the new job requirement. " + ex.Message);
            }
        }

        public int JobCount()
        {
            return Jobs.Count();
        }

        //public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        //{
        //    args.Cancel = false;
        //    await Task.CompletedTask;
        //}

    }
}

