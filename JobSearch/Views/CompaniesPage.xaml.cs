using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JobSearch.Views
{
    public sealed partial class CompaniesPage : Page
    {
        public static CompaniesPage Instance { get; private set; }

        public CompaniesPage()
        {
            InitializeComponent();
            //NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Instance = this;
        }

        public void ToggleAddCompanyForm()
        {
            AddCompanyForm.Visibility = (AddCompanyForm.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void AddCompany_Clicked(object sender, RoutedEventArgs e)
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

                AddCompanyForm.Visibility = Visibility.Collapsed;
                CompaniesList.SelectedIndex = ViewModel.CompanyCount() - 1;
            }
            catch (ArgumentNullException ex)
            {

            }
        }

        private void FillForm_Clicked(object sender, RoutedEventArgs e)
        {
            NameBox.Text = "Pizza Hut";
            DomainBox.Text = "Food Service";
            SizeBox.Text = "large";
            NotesBox.Text = "good place to eat pizza";
            WebsiteBox.Text = "https://www.pizzahut.com";
            LinkedInBox.Text = "https://www.linkedin.com/";
            GlassdoorBox.Text = "https://www.glassdoor.com/index.htm";
            StreetAddressBox.Text = "1453 Waverly Lane";
            CityBox.Text = "Houston";
            StateBox.Text = "TX";
            ZipCodeBox.Text = "77034";
        }
    }
}
