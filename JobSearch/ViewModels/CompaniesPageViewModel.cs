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
    public class CompaniesPageViewModel : ViewModelBase
    {
        Services.DatabaseService.DatabaseService db;

        public CompaniesPageViewModel()
        {
            _instance = this;
            db = Services.DatabaseService.DatabaseService.GetDB();
            Companies = new ObservableCollection<Company>(db.Companies);
            Selected = (Companies.Count > 0) ? Companies.First() : null;
        }

        private static CompaniesPageViewModel _instance;
        public static CompaniesPageViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CompaniesPageViewModel();
                return _instance;
            }
            set { _instance = value; }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            return Task.CompletedTask;
        }

        private ObservableCollection<Company> _companies;
        public ObservableCollection<Company> Companies
        {
            get { return _companies; }
            set { Set(ref _companies, value); }
        }

        Company _selected = default(Company);
        public Company Selected
        {
            get { return _selected; }
            set
            {
                var company = value as Company;
                Set(ref _selected, company);
            }
        }

        Job _selectedJob = default(Job);
        public object SelectedJob
        {
            get { return _selectedJob; }
            set
            {
                var job = value as Job;
                Set(ref _selectedJob, job);
            }
        }

        public void AddCompany(string name, string domain, string size, string notes
                , string website, string linkedIn, string glassdoor
                , string streetAddress, string city, string state, int? zipCode)
        {
            try
            {
                Company newCompany = new Company()
                {
                    Name = String.IsNullOrWhiteSpace(name) ? null : name,
                    Domain = String.IsNullOrWhiteSpace(domain) ? null : domain,
                    Size = String.IsNullOrWhiteSpace(size) ? null : size,
                    Notes = String.IsNullOrWhiteSpace(notes) ? null : notes,
                    Website = String.IsNullOrWhiteSpace(website) ? null : website,
                    LinkedIn = String.IsNullOrWhiteSpace(linkedIn) ? null : linkedIn,
                    Glassdoor = String.IsNullOrWhiteSpace(glassdoor) ? null : glassdoor,
                    StreetAddress = String.IsNullOrWhiteSpace(streetAddress) ? null : streetAddress,
                    City = String.IsNullOrWhiteSpace(city) ? null : city,
                    State = String.IsNullOrWhiteSpace(state) ? null : state,
                    ZipCode = zipCode,
                    Jobs = new ObservableCollection<Job>()
                };

                db.AddCompany(newCompany);
                Companies.Add(newCompany);
                RaisePropertyChanged(nameof(Companies));
            }
            catch (SQLite.Net.NotNullConstraintViolationException ex)
            {
                throw new ArgumentNullException("Required fields were omitted for the new company");
            }
        }

        public IEnumerable<Company> Search(string searchText)
            => db.Companies.Where(company => company.Name.ToLower().Contains(searchText.ToLower()));

        public void Filter(string filterText)
        {
            IEnumerable<Company> result = db.Companies.Where(company => company.Name.ToLower().Contains(filterText.ToLower()));
            Companies.Clear();
            foreach (Company company in result)
                Companies.Add(company);
        }

        public void Select(object company)
            => Selected = company as Company;

        public int CompanyCount()
            => Companies.Count();

    }
}

