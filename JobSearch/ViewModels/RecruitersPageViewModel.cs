using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using JobSearch.Models;

namespace JobSearch.ViewModels
{
    public class RecruitersPageViewModel : ViewModelBase
    {
        Services.DatabaseService.DatabaseService db;

        public RecruitersPageViewModel()
        {
            _instance = this;
            db = Services.DatabaseService.DatabaseService.GetDB();
        }

        private static RecruitersPageViewModel _instance;
        public static RecruitersPageViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RecruitersPageViewModel();
                return _instance;
            }
            set { _instance = value; }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            Recruiters = new ObservableCollection<Recruiter>(db.Recruiters);
            Selected = (Recruiters.Count > 0) ? Recruiters.First() : null;

            return Task.CompletedTask;
        }

        private ObservableCollection<Recruiter> _recruiters;
        public ObservableCollection<Recruiter> Recruiters
        {
            get
            {
                return _recruiters;
            }
            set { Set(ref _recruiters, value); }
        }

        Recruiter _selected = default(Recruiter);
        public object Selected
        {
            get { return _selected; }
            set
            {
                var recruiter = value as Recruiter;
                Set(ref _selected, recruiter);
            }
        }

        public void AddRecruiter(string name, string email, string title, string notes)
        {
            try
            {
                Recruiter newRecruiter = new Recruiter()
                {
                    Name = String.IsNullOrWhiteSpace(name) ? null : name,
                    Email = String.IsNullOrWhiteSpace(email) ? null : email,
                    Title = String.IsNullOrWhiteSpace(title) ? null : title,
                    Notes = String.IsNullOrWhiteSpace(notes) ? null : notes,
                    Jobs = new ObservableCollection<Job>()
                };

                db.AddRecruiter(newRecruiter);
                Recruiters.Add(newRecruiter);
                RaisePropertyChanged(nameof(Recruiters));
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex)
            {
                throw new ArgumentNullException("Required fields were omitted for the new recruiter");
            }
        }

        public int RecruiterCount()
        {
            return Recruiters.Count();
        }

    }
}

