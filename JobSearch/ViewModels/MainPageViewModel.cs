using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Services.NavigationService;
using Template10.Mvvm;
using JobSearch.Models;
using System.Text.RegularExpressions;

namespace JobSearch.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private const string LIST_ITEM_LEADING_SEPARATOR = @"^\s*[-·•*]\s*\b";
        private const string LIST_ITEM_SEPARATOR = @"[\n\r]+\s*[-·•*]?\s*\b";
        public enum ModelTypes { Company, Recruiter, Position, Area };
        Services.DatabaseService.DatabaseService db;

        public MainPageViewModel()
        {
            _instance = this;
            db = Services.DatabaseService.DatabaseService.GetDB();

            Jobs = new ObservableCollection<Job>(db.Jobs.OrderByDescending(job => job.DateApplied).ThenByDescending(job => job.JobId));
            Selected = (Jobs.Count > 0) ? Jobs.First() : null;
            Requirements = new ObservableCollection<Job_Requirement>(db.Requirements);
            Responsibilities = new ObservableCollection<Job_Responsibility>(db.Responsibilities);
            Tests = new ObservableCollection<Job_Test>(db.Tests);
            Interviews = new ObservableCollection<Job_Interview>(db.Interviews);
            Communications = new ObservableCollection<Job_Communication>(db.Communications);

            _searchByCompany = true;
            _searchByRecruiter = false;
            _searchByPosition = false;
            _searchByArea = false;

            _sortByDateApplied = true;
            _sortByPosition = false;
            _sortByDatePosted = false;

            _showAppliedJobs = true;
            _showInterestedJobs = true;
            _showWaitingOnMeToCommunicateJobs = true;
            _showWaitingOnMeJobCommunicateJobs = true;
            _showUpcomingTestJobs = true;
            _showUpcomingInterviewJobs = true;
            _showDeclinedToOfferJobs = true;
            _showOfferedJobs = true;
            _showOfferDeclinedJobs = true;
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

        public void UpdateSelected() => db.Update(Selected as Job);

        private bool _searchByCompany;
        public bool SearchByCompany
        {
            get { return _searchByCompany; }
            set { Set(ref _searchByCompany, value); }
        }

        private bool _searchByRecruiter;
        public bool SearchByRecruiter
        {
            get { return _searchByRecruiter; }
            set { Set(ref _searchByRecruiter, value); }
        }

        private bool _searchByPosition;
        public bool SearchByPosition
        {
            get { return _searchByPosition; }
            set { Set(ref _searchByPosition, value); }
        }

        private bool _searchByArea;
        public bool SearchByArea
        {
            get { return _searchByArea; }
            set { Set(ref _searchByArea, value); }
        }

        private bool _sortByDateApplied;
        public bool SortByDateApplied
        {
            get { return _sortByDateApplied; }
            set
            {
                if (value)
                    SortByPosition = SortByDatePosted = false;
                Set(ref _sortByDateApplied, value);
            }
        }

        private bool _sortByDatePosted;
        public bool SortByDatePosted
        {
            get { return _sortByDatePosted; }
            set
            {
                if (value)
                    SortByPosition = SortByDateApplied = false;
                Set(ref _sortByDatePosted, value);
            }
        }

        private bool _sortByPosition;
        public bool SortByPosition
        {
            get { return _sortByPosition; }
            set
            {
                if (value)
                    SortByDateApplied = SortByDatePosted = false;
                Set(ref _sortByPosition, value);
            }
        }

        private bool _showAppliedJobs;
        public bool ShowAppliedJobs
        {
            get { return _showAppliedJobs; }
            set { Set(ref _showAppliedJobs, value); }
        }

        private bool _showInterestedJobs;
        public bool ShowInterestedJobs
        {
            get { return _showInterestedJobs; }
            set { Set(ref _showInterestedJobs, value); }
        }

        private bool _showWaitingOnMeToCommunicateJobs;
        public bool ShowWaitingOnMeToCommunicateJobs
        {
            get { return _showWaitingOnMeToCommunicateJobs; }
            set { Set(ref _showWaitingOnMeToCommunicateJobs, value); }
        }

        private bool _showWaitingOnMeJobCommunicateJobs;
        public bool ShowWaitingOnMeJobCommunicateJobs
        {
            get { return _showWaitingOnMeJobCommunicateJobs; }
            set { Set(ref _showWaitingOnMeJobCommunicateJobs, value); }
        }

        private bool _showUpcomingTestJobs;
        public bool ShowUpcomingTestJobs
        {
            get { return _showUpcomingTestJobs; }
            set { Set(ref _showUpcomingTestJobs, value); }
        }

        private bool _showUpcomingInterviewJobs;
        public bool ShowUpcomingInterviewJobs
        {
            get { return _showUpcomingInterviewJobs; }
            set { Set(ref _showUpcomingInterviewJobs, value); }
        }

        private bool _showDeclinedToOfferJobs;
        public bool ShowDeclinedToOfferJobs
        {
            get { return _showDeclinedToOfferJobs; }
            set { Set(ref _showDeclinedToOfferJobs, value); }
        }

        private bool _showOfferedJobs;
        public bool ShowOfferedJobs
        {
            get { return _showOfferedJobs; }
            set { Set(ref _showOfferedJobs, value); }
        }

        private bool _showOfferDeclinedJobs;
        public bool ShowOfferDeclinedJobs
        {
            get { return _showOfferDeclinedJobs; }
            set { Set(ref _showOfferDeclinedJobs, value); }
        }

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

        public void FlagSelection(bool flagged)
        {
            _selected.Flagged = flagged;
            db.Update(_selected);
        }

        public bool? IsSelectionFlagged()
            => _selected?.Flagged;

        public void AddJob(string position, int? minSalary, int? maxSalary, bool? isSalaryEstimate, string company, string recruiter
                , string notes, int? yearsExperienceNeeded, string employmentService, string employmentServiceJobLink
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
                    IsSalaryEstimate = isSalaryEstimate ?? false,
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
                Jobs.Insert(0, newJob);
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex)
            {
                throw new ArgumentNullException("Required fields were omitted for the new job");
            }
        }

        public void DeleteSelectedJob()
        {
            Job currentJob = Selected as Job;
            db.DeleteJob(currentJob);
            Jobs.Remove(currentJob);
            Selected = null;
        }

        public void AddRequirement(string requirements)
        {
            try
            {
                Job currentJob = Selected as Job;

                requirements = Regex.Replace(requirements, LIST_ITEM_LEADING_SEPARATOR, "");
                foreach (string req in Regex.Split(requirements, LIST_ITEM_SEPARATOR))
                {
                    if (!String.IsNullOrWhiteSpace(req))
                    {
                        Job_Requirement newRequirement = new Job_Requirement()
                        {
                            Requirement = req
                        };

                        db.AddJobRequirement(newRequirement, currentJob.JobId);
                        Requirements.Add(newRequirement);
                    }
                }

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

        public void AddResponsibility(string responsibilities)
        {
            try
            {
                Job currentJob = Selected as Job;

                responsibilities = Regex.Replace(responsibilities, LIST_ITEM_LEADING_SEPARATOR, "");
                foreach (string resp in Regex.Split(responsibilities, LIST_ITEM_SEPARATOR))
                {
                    if (!String.IsNullOrWhiteSpace(resp))
                    {
                        Job_Responsibility newResponsibility = new Job_Responsibility()
                        {
                            Responsibility = resp
                        };

                        db.AddJobResponsibility(newResponsibility, currentJob.JobId);
                        Responsibilities.Add(newResponsibility);
                    }
                }

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

        public void EditInterview(int interviewId, string via, string interviewer, DateTime? date, TimeSpan time, string notes)
        {
            try
            {
                Job currentJob = Selected as Job;
                Job_Interview jobInterview = currentJob.Interviews.Where(interview => interview.InterviewId == interviewId).Single();
                jobInterview.Via = via;
                jobInterview.Interviewer = interviewer;
                jobInterview.DateAndTime = date?.Add(time);
                jobInterview.Notes = notes;

                db.Update(jobInterview);
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex)
            {
                throw new ArgumentNullException("Required fields were omitted for the edited interview");
            }
        }

        public void DeleteInterview(int interviewId)
        {
            Job currentJob = Selected as Job;
            Job_Interview jobInterview = currentJob.Interviews.Where(interview => interview.InterviewId == interviewId).Single();

            db.DeleteJobInterview(jobInterview, currentJob.JobId);
            Interviews.Remove(jobInterview);
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

        public void EditCommunication(int communicationId, string from, string to, string subject, string via, DateTime? date, TimeSpan time, string description)
        {
            try
            {
                Job currentJob = Selected as Job;
                Job_Communication jobCommunication = currentJob.Communications.Where(communication => communication.CommunicationId == communicationId).Single();
                jobCommunication.From = from;
                jobCommunication.To = to;
                jobCommunication.Subject = subject;
                jobCommunication.Via = via;
                jobCommunication.DateAndTime = date?.Add(time);
                jobCommunication.Description = description;

                db.Update(jobCommunication);
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex)
            {
                throw new ArgumentNullException("Required fields were omitted for the edited interview");
            }
        }

        public void DeleteCommunication(int communicationId)
        {
            Job currentJob = Selected as Job;
            Job_Communication jobCommunication = currentJob.Communications.Where(communication => communication.CommunicationId == communicationId).Single();

            db.DeleteJobCommunication(jobCommunication, currentJob.JobId);
            Communications.Remove(jobCommunication);
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

        public IEnumerable<SearchResult> SearchJobs(string searchText)
        {
            ISet<SearchResult> result = new HashSet<SearchResult>();

            if (SearchByCompany)
                foreach (Company company in db.Companies.Where(company => company.Name.ToLower().Contains(searchText.ToLower())))
                    result.Add(new SearchResult(company.Name, MainPageViewModel.ModelTypes.Company));
            if (SearchByRecruiter)
                foreach (Recruiter recruiter in db.Recruiters.Where(recruiter => recruiter.Name.ToLower().Contains(searchText.ToLower())))
                    result.Add(new SearchResult(recruiter.Name, MainPageViewModel.ModelTypes.Recruiter));
            if (SearchByPosition)
                foreach (Job job in db.Jobs.Where(job => job.Position.ToLower().Contains(searchText.ToLower())))
                    result.Add(new SearchResult(job.Position, MainPageViewModel.ModelTypes.Position));
            if (SearchByArea)
                foreach (Job job in db.Jobs.Where(job => job.Area.ToLower().Contains(searchText.ToLower())))
                    result.Add(new SearchResult(job.Area, MainPageViewModel.ModelTypes.Area));

            return result;
        }

        public void FilterJobs(string filterText)
        {
            ISet<Job> matchingJobs = new HashSet<Job>();
            IOrderedEnumerable<Job> matchingJobsOrdered;
            string lowerFilterText = filterText.ToLower();

            if (string.IsNullOrEmpty(filterText))
                matchingJobs.UnionWith(db.Jobs);
            else
            {
                if (SearchByCompany)
                    matchingJobs.UnionWith(db.Jobs.Where(job => job.Company.Name.ToLower().Contains(lowerFilterText)));
                if (SearchByRecruiter)
                    matchingJobs.UnionWith(db.Jobs.Where(job => job.Recruiter?.Name.ToLower().Contains(lowerFilterText) ?? false));
                if (SearchByPosition)
                    matchingJobs.UnionWith(db.Jobs.Where(job => job.Position.ToLower().Contains(lowerFilterText)));
                if (SearchByArea)
                    matchingJobs.UnionWith(db.Jobs.Where(job => job.Area.ToLower().Contains(lowerFilterText)));
            }

            if (SortByDateApplied)
                matchingJobsOrdered = matchingJobs.OrderByDescending(job => job.DateApplied).ThenByDescending(job => job.JobId);
            else if (SortByDatePosted)
                matchingJobsOrdered = matchingJobs.OrderByDescending(job => job.DatePosted).ThenByDescending(job => job.JobId);
            else // (SortByPosition)
                matchingJobsOrdered = matchingJobs.OrderBy(job => job.Position).ThenByDescending(job => job.JobId);

            Jobs.Clear();
            foreach (Job job in matchingJobsOrdered)
                Jobs.Add(job);
        }

        public IEnumerable<Company> SearchCompanies(string searchText)
        {
            ISet<Company> result = new HashSet<Company>();
            foreach (Company company in db.Companies.Where(cmpny => cmpny.Name.ToLower().Contains(searchText.ToLower())))
                result.Add(company);

            return result;
        }

        public bool CompanyExists(string companyName)
            => db.Companies.Where(cmpny => cmpny.Name.ToLower().Equals(companyName.ToLower())).Count() > 0;

        public void SortBy(string sortBy)
        {
            List<Job> shownJobs = new List<Job>(Jobs);
            Jobs.Clear();

            if (sortBy == "Date Applied")
                foreach (Job job in shownJobs.OrderByDescending(job => job.DateApplied).ThenByDescending(job => job.JobId))
                    Jobs.Add(job);
            else if (sortBy == "Date Posted")
                foreach (Job job in shownJobs.OrderByDescending(job => job.DatePosted).ThenByDescending(job => job.JobId))
                    Jobs.Add(job);
            else if (sortBy == "Position")
                foreach (Job job in shownJobs.OrderBy(job => job.Position).ThenByDescending(job => job.JobId))
                    Jobs.Add(job);
        }

        public void Select(object job)
            => Selected = job as Job;

        public int JobCount()
            => Jobs.Count();

        //public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        //{
        //    args.Cancel = false;
        //    await Task.CompletedTask;
        //}

    }

    public class SearchResult
    {
        public string Result { get; set; }
        public MainPageViewModel.ModelTypes ModelType { get; set; }

        public SearchResult(string result, MainPageViewModel.ModelTypes modelType)
        {
            Result = result;
            ModelType = modelType;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is SearchResult))
                return false;

            SearchResult other = obj as SearchResult;
            return Result.Equals(other.Result) && ModelType.Equals(other.ModelType);
        }

        public override int GetHashCode()
            => Result.GetHashCode() + ModelType.GetHashCode();
    }
}

