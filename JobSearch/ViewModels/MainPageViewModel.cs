using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using JobSearch.Views;
using Template10.Services.NavigationService;
using Template10.Common;
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
            //Value = "Designtime value";
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

        //string _Value = "Gas";
        //public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            //if (suspensionState.Any())
            //{
            //    Value = suspensionState[nameof(Value)]?.ToString();
            //}
            //await Task.CompletedTask;

            Jobs = new ObservableCollection<Job>(db.Jobs);
            Selected = Jobs?.First();
            Requirements = new ObservableCollection<Job_Requirement>(db.Requirements);
            Responsibilities = new ObservableCollection<Job_Responsibility>(db.Responsibilities);
            Tests = new ObservableCollection<Job_Test>(db.Tests);
            Interviews = new ObservableCollection<Job_Interview>(db.Interviews);

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

        //public DelegateCommand SwitchToControlCommand =
        //    new DelegateCommand(() => BootStrapper.Current.NavigationService.Navigate(typeof(MainPage))); // was originally MasterDetailsPage

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

        public void AddJob(string position, string company, string recruiter, string notes
                , string employmentService, bool? appliedViaWebsite, bool? appliedViaEmail, DateTime? datePosted, DateTime? dateApplied
                , string streetAddress, string city, string state, int? zipCode)
        {
            try
            {
                Job newJob = new Job()
                {
                    Position = String.IsNullOrWhiteSpace(position) ? null : position,
                    Notes = String.IsNullOrWhiteSpace(notes) ? null : notes,
                    EmploymentService = String.IsNullOrWhiteSpace(employmentService) ? null : employmentService,
                    AppliedViaWebsite = appliedViaWebsite ?? false,
                    AppliedViaEmail = appliedViaEmail ?? false,
                    DatePosted = datePosted,
                    DateApplied = dateApplied,
                    StreetAddress = String.IsNullOrWhiteSpace(streetAddress) ? null : streetAddress,
                    City = String.IsNullOrWhiteSpace(city) ? null : city,
                    State = String.IsNullOrWhiteSpace(state) ? null : state,
                    ZipCode = zipCode
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
                //currentJob.Requirements.Add(newRequirement);
                Requirements.Add(newRequirement);
                RaisePropertyChanged(nameof(Jobs));
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex)
            {
                throw new ArgumentNullException("Required fields were omitted for the new job requirement. " + ex.Message);
            }
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

        public void AddTest(string type, DateTime date, TimeSpan time, string notes)
        {
            try
            {
                Job currentJob = Selected as Job;

                Job_Test newTest = new Job_Test()
                {
                    Type = String.IsNullOrWhiteSpace(type) ? null : type,
                    DateAndTime = date.Add(time),
                    Notes = String.IsNullOrWhiteSpace(notes) ? null : notes
                };

                db.AddJobTest(newTest, currentJob.JobId);
                Tests.Add(newTest);
                RaisePropertyChanged(nameof(Jobs));
            }
            // note that exceptions must be caught here because the calling method code is not accessible
            catch (SQLite.Net.NotNullConstraintViolationException ex) { System.Diagnostics.Debug.Write(ex.Message); }
        }

        public void AddInterview(string via, string interviewer, DateTime date, TimeSpan time, string notes)
        {
            try
            {
                Job currentJob = Selected as Job;

                Job_Interview newInterview = new Job_Interview()
                {
                    Via = String.IsNullOrWhiteSpace(via) ? null : via,
                    Interviewer = String.IsNullOrWhiteSpace(interviewer) ? null : interviewer,
                    DateAndTime = date.Add(time),
                    Notes = String.IsNullOrWhiteSpace(notes) ? null : notes
                };

                db.AddJobInterview(newInterview, currentJob.JobId);
                Interviews.Add(newInterview);
                RaisePropertyChanged(nameof(Jobs));
            }
            // note that exceptions must be caught here because the calling method code is not accessible
            catch (SQLite.Net.NotNullConstraintViolationException ex) { System.Diagnostics.Debug.Write(ex.Message); }
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

        //public void GotoDetailsPage() =>
        //    NavigationService.Navigate(typeof(Views.DetailPage), Value);

        //public void GotoSettings() =>
        //    NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        //public void GotoPrivacy() =>
        //    NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        //public void GotoAbout() =>
        //    NavigationService.Navigate(typeof(Views.SettingsPage), 2);

    }
}

