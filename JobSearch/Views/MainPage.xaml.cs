//using System;
//using JobSearch.ViewModels;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Windows;
using Windows.UI.Xaml.Controls.Primitives;
using JobSearch.Controls;
//using Windows.UI.Xaml.Navigation;
//using System.Collections.ObjectModel;

namespace JobSearch.Views
{
    public sealed partial class MainPage : Page
    {
        public static MainPage Instance { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            //NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Instance = this;
        }

        public void ToggleAddJobForm()
        {
            AddJobForm.Visibility = (AddJobForm.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void AddJob_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AddJob(
                    position: PositionBox.Text,
                    company: CompanyBox.Text,
                    recruiter: RecruiterBox.Text,
                    notes: NotesBox.Text,
                    employmentService: EmploymentServiceBox.Text,
                    appliedViaWebsite: AppliedViaWebsiteBox.IsChecked,
                    appliedViaEmail: AppliedViaEmailBox.IsChecked,
                    datePosted: DatePostedBox.Date?.Date,
                    dateApplied: DateAppliedBox.Date?.Date,
                    streetAddress: StreetAddressBox.Text,
                    city: CityBox.Text,
                    state: StateBox.Text,
                    zipCode: (ZipCodeBox.Text.Trim().Length > 0) ? int.Parse(ZipCodeBox.Text) as int? : null
                    //ignore: 
                );

                AddJobForm.Visibility = Visibility.Collapsed;
                JobsList.SelectedIndex = ViewModel.JobCount() - 1;
            }
            catch (ArgumentNullException ex)
            {
            }
        }

        private void FillForm_Clicked(object sender, RoutedEventArgs e)
        {
            PositionBox.Text = "Web Developer";
            CompanyBox.Text = "GHG Corporation";
            RecruiterBox.Text = "Ira D'Silva";
            NotesBox.Text = "this is test data and not a real job";
            EmploymentServiceBox.Text = "Zip Recruiter";
            AppliedViaWebsiteBox.IsChecked = true;
            DatePostedBox.Date = new System.DateTimeOffset(new System.DateTime(2016, 7, 5));
            DateAppliedBox.Date = new System.DateTimeOffset(new System.DateTime(2016, 7, 13));
            StreetAddressBox.Text = "1453 Waverly Lane";
            CityBox.Text = "Houston";
            StateBox.Text = "TX";
            ZipCodeBox.Text = "77034";
        }

        private void ShowTestPopup(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(sender as DependencyObject);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is Popup)
                {
                    Popup popup = child as Popup;
                    popup.IsOpen = true;
                    (popup.Child as TestForm).Focus();
                }
            }
        }
    }
}
