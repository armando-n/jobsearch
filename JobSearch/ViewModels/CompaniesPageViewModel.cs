using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using JobSearch.Views;
using Template10.Common;
using Template10.Mvvm;
using JobSearch.Models;

namespace JobSearch.ViewModels
{
    public class CompaniesPageViewModel : ViewModelBase
    {
        Services.DatabaseService.DatabaseService db;

        public CompaniesPageViewModel()
        {
            db = Services.DatabaseService.DatabaseService.GetDB();
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            Companies = new ObservableCollection<Company>(db.Companies);
            Selected = Companies?.First();

            return Task.CompletedTask;
        }

        private ObservableCollection<Company> _companies;
        public ObservableCollection<Company> Companies
        {
            get
            {
                return _companies;
            }
            set { Set(ref _companies, value); }
        }

        public DelegateCommand SwitchToControlCommand =
            new DelegateCommand(() => BootStrapper.Current.NavigationService.Navigate(typeof(CompaniesPage))); // was originally MasterDetailsPage

        Company _selected = default(Company);
        public object Selected
        {
            get { return _selected; }
            set
            {
                var company = value as Company;
                Set(ref _selected, company);
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
                    Linkedin = String.IsNullOrWhiteSpace(linkedIn) ? null : linkedIn,
                    Glassdoor = String.IsNullOrWhiteSpace(glassdoor) ? null : glassdoor,
                    StreetAddress = String.IsNullOrWhiteSpace(streetAddress) ? null : streetAddress,
                    City = String.IsNullOrWhiteSpace(city) ? null : city,
                    State = String.IsNullOrWhiteSpace(state) ? null : state,
                    ZipCode = zipCode
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

        public int CompanyCount()
        {
            return Companies.Count();
        }

    }
}

