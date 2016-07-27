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
using JobSearch.Services.DatabaseService;

namespace JobSearch.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        Services.MessageService.MessageService _messageService;

        public MainPageViewModel()
        {
            //if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            //{
                _messageService = new Services.MessageService.MessageService();
                Jobs = new ObservableCollection<Job>(DatabaseService.GetDB().Jobs);
                //Jobs.Count();
                //Value = "Designtime value";
            //}
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

            Messages = _messageService.GetMessages();
            Selected = Messages?.FirstOrDefault();

            //Jobs = new ObservableCollection<Job>();
            //var dbService = Services.DatabaseService.DatabaseService.getDB();
            //List<Job> listOfJobs = dbService.Jobs;
            //foreach (Job job in listOfJobs)
            //    Jobs.Add(job);

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

        ObservableCollection<Models.Message> _messages = default(ObservableCollection<Models.Message>);
        public ObservableCollection<Models.Message> Messages { get { return _messages; } private set { Set(ref _messages, value); } }

        private ObservableCollection<Models.Job> _jobs;
        public ObservableCollection<Models.Job> Jobs
        {
            get
            {
                //if (_jobs == null)
                //    _jobs = new ObservableCollection<Job>(DatabaseService.GetDB().Jobs);
                return _jobs;
                //_jobs = Services.DatabaseService.DatabaseService.getDB().Jobs;
                //return _jobs ?? new ObservableCollection<Job>(DatabaseService.GetDB().Jobs);
                //return DatabaseService.GetDB().Jobs;
            }
            set { /*_jobs = value;*/ Set(ref _jobs, value); }
            //get
            //{
            //    return Services.DatabaseService.DatabaseService.getDB().Jobs;
            //}
            //private set { }
        }

        string _searchText = default(string);
        public string SearchText { get { return _searchText; } set { Set(ref _searchText, value); } }

        public DelegateCommand SwitchToControlCommand =
            new DelegateCommand(() => BootStrapper.Current.NavigationService.Navigate(typeof(MainPage))); // was originally MasterDetailsPage

        Models.Job _selected = default(Models.Job);
        public object Selected
        {
            get { return _selected; }
            set
            {
                var job = value as Models.Job;
                Set(ref _selected, job);
                //if (job != null)
                //    job.IsRead = true;
            }
        }

        public void AddJob(string position, string company, string recruiter, string notes
                , string employmentService, bool? appliedViaWebsite, bool? appliedViaEmail, DateTime? datePosted, DateTime? dateApplied
                , string streetAddress, string city, string state, int? zipCode)
        {
            Job newJob = new Job()
            {
                Position = position,
                Notes = notes,
                EmploymentService = employmentService,
                AppliedViaWebsite = appliedViaWebsite ?? false,
                AppliedViaEmail = appliedViaEmail ?? false,
                DatePosted = datePosted,
                DateApplied = dateApplied,
                StreetAddress = streetAddress,
                City = city,
                State = state,
                ZipCode = zipCode
            };

            DatabaseService.GetDB().AddJob(newJob, company, recruiter);
            Jobs.Add(newJob);
            RaisePropertyChanged(nameof(Jobs));
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

