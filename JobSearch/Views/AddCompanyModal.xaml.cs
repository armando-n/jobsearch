using JobSearch.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JobSearch.Views
{
    public sealed partial class AddCompanyModal : UserControl
    {
        private CompaniesPageViewModel ViewModel;
        private AddJobModal CallingModal;
        private string CompanyName;

        public AddCompanyModal(AddJobModal callingModal = null, string companyName = null)
        {
            InitializeComponent();
            this.DataContext = ViewModel = CompaniesPageViewModel.Instance;
            this.CallingModal = callingModal;
            this.CompanyName = companyName;
            this.NameBox.GotFocus += ModalFinishedLoading;
        }

        public void ModalFinishedLoading(object sender, RoutedEventArgs e)
        {
            if (CompanyName != null)
            {
                NameBox.Text = CompanyName;
                DomainBox.Focus(FocusState.Programmatic);
                CompanyName = null;
            }
        }

        public void AddCompany(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AddCompany(
                    name: NameBox.Text,
                    domain: DomainBox.Text,
                    size: SizeBox.Text,
                    notes: NotesBox.Text,
                    website: WebsiteBox.Text,
                    linkedIn: LinkedInBox.Text,
                    glassdoor: GlassdoorBox.Text,
                    streetAddress: StreetAddressBox.Text,
                    city: CityBox.Text,
                    state: StateBox.Text,
                    zipCode: (ZipCodeBox.Text.Trim().Length > 0) ? int.Parse(ZipCodeBox.Text) as int? : null
                );

                CloseModal(null, null);
            }
            catch (ArgumentNullException ex) { }
        }

        private void CloseModal(object sender, RoutedEventArgs e)
        {
            if (CallingModal != null)
                BootStrapper.Current.ModalContent = CallingModal;
            else
            {
                BootStrapper.Current.ModalDialog.IsModal = false;
                CompaniesPage.Instance.SelectLast();
            }
        }

    }
}

