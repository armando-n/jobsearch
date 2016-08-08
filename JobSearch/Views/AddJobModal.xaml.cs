using JobSearch.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JobSearch.Views
{
    public sealed partial class AddJobModal : UserControl
    {
        private MainPageViewModel ViewModel;

        public AddJobModal()
        {
            InitializeComponent();
            this.DataContext = ViewModel = MainPageViewModel.Instance;
        }

        public void AddJob_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AddJob(
                    position: PositionBox.Text,
                    minSalary: (MinSalaryBox.Text.Trim().Length > 0) ? int.Parse(MinSalaryBox.Text) as int? : null,
                    maxSalary: (MaxSalaryBox.Text.Trim().Length > 0) ? int.Parse(MaxSalaryBox.Text) as int? : null,
                    company: CompanyBox.Text,
                    recruiter: RecruiterBox.Text,
                    notes: NotesBox.Text,
                    yearsExperienceNeeded: (YearsExperienceNeededBox.Text.Trim().Length > 0) ? int.Parse(YearsExperienceNeededBox.Text) as int? : null,
                    employmentService: EmploymentServiceBox.Text,
                    employmentServiceJobLink: EmploymentServiceJobLinkBox.Text,
                    appliedViaService: AppliedViaServiceBox.IsChecked,
                    appliedViaWebsite: AppliedViaWebsiteBox.IsChecked,
                    appliedViaEmail: AppliedViaEmailBox.IsChecked,
                    datePosted: DatePostedBox.Date?.Date,
                    dateApplied: DateAppliedBox.Date?.Date,
                    streetAddress: StreetAddressBox.Text,
                    city: CityBox.Text,
                    state: StateBox.Text,
                    zipCode: (ZipCodeBox.Text.Trim().Length > 0) ? int.Parse(ZipCodeBox.Text) as int? : null,
                    area: AreaBox.Text,
                    active: ActiveBox.IsChecked,
                    flagged: FlagBox.IsChecked
                );

                BootStrapper.Current.ModalDialog.IsModal = false;
                MainPage.Instance.SelectLast();
            }
            catch (ArgumentNullException ex) { }
        }

        private void CancelJob_Clicked(object sender, RoutedEventArgs e) => BootStrapper.Current.ModalDialog.IsModal = false;

        private void AppliedBox_Checked(object sender, RoutedEventArgs e)
        {
            AppliedArea.Visibility = Visibility.Visible;
            if (NotesBox.Visibility == Visibility.Collapsed)
                CancelJob.SetValue(RelativePanel.BelowProperty, AppliedArea);
        }

        private void AppliedBox_Unchecked(object sender, RoutedEventArgs e)
        {
            AppliedArea.Visibility = Visibility.Collapsed;
            AppliedViaServiceBox.IsChecked = false;
            AppliedViaWebsiteBox.IsChecked = false;
            AppliedViaEmailBox.IsChecked = false;
            if (CancelJob.GetValue(RelativePanel.BelowProperty) == AppliedArea)
                CancelJob.ClearValue(RelativePanel.BelowProperty);
        }

        private void NotesBox_Checked(object sender, RoutedEventArgs e)
        {
            NotesBox.Visibility = Visibility.Visible;
            if (AppliedArea.Visibility == Visibility.Visible)
                CancelJob.ClearValue(RelativePanel.BelowProperty);
        }

        private void NotesBox_Unchecked(object sender, RoutedEventArgs e)
        {
            NotesBox.Visibility = Visibility.Collapsed;
            NotesBox.Text = "";
            if (AppliedArea.Visibility == Visibility.Visible)
                CancelJob.SetValue(RelativePanel.BelowProperty, AppliedArea);
        }
    }
}

