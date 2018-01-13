using JobSearch.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Common;
using Windows.UI.Popups;
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
            if (!string.IsNullOrWhiteSpace(CompanySuggestBox.Text) && !ViewModel.CompanyExists(CompanySuggestBox.Text))
            {
                ShowAddCompanyConfirmationDialog();
                return;
            }

            try
            {
                ViewModel.AddJob(
                    position: PositionBox.Text,
                    minSalary: (MinSalaryBox.Text.Trim().Length > 0) ? int.Parse(MinSalaryBox.Text) as int? : null,
                    maxSalary: (MaxSalaryBox.Text.Trim().Length > 0) ? int.Parse(MaxSalaryBox.Text) as int? : null,
                    isSalaryEstimate: IsSalaryEstimateBox.IsChecked,
                    company: CompanySuggestBox.Text,
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
                MainPage.Instance.SelectFirst();
            }
            catch (ArgumentNullException ex) { }
        }

        private void CancelJob_Clicked(object sender, RoutedEventArgs e)
            => BootStrapper.Current.ModalDialog.IsModal = false;

        private void Company_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(CompanySuggestBox.Text))
                    CompanySuggestBox.ItemsSource = null;
                else
                    CompanySuggestBox.ItemsSource = ViewModel.SearchCompanies(CompanySuggestBox.Text);
            }
        }

        private async void ShowAddCompanyConfirmationDialog()
        {
            var dialog = new MessageDialog($"The company \"{CompanySuggestBox.Text}\" was not found. You must add it to continue.");
            dialog.Commands.Add(new UICommand("Continue", new UICommandInvokedHandler(this.ConfirmationHandler)));
            dialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(this.ConfirmationHandler)));
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            await dialog.ShowAsync();
        }

        private void ConfirmationHandler(IUICommand command)
        {
            if (command.Label == "Continue")
                BootStrapper.Current.ModalContent = new AddCompanyModal(this, CompanySuggestBox.Text);
        }

        private void CityText_Changed(object sender, TextChangedEventArgs e)
        {
            if (AreaBox.Text == CityBox.Text.Substring(0, CityBox.Text.Length - 1))
                AreaBox.Text = CityBox.Text;
        }

        private void AppliedBox_Checked(object sender, RoutedEventArgs e)
        {
            AppliedArea.Visibility = Visibility.Visible;
            if (NotesBox.Visibility == Visibility.Collapsed)
                CancelJob.SetValue(RelativePanel.BelowProperty, AppliedArea);
            DateAppliedBox.Date = DateTime.Today;
        }

        private void AppliedBox_Unchecked(object sender, RoutedEventArgs e)
        {
            AppliedArea.Visibility = Visibility.Collapsed;
            AppliedViaServiceBox.IsChecked = false;
            AppliedViaWebsiteBox.IsChecked = false;
            AppliedViaEmailBox.IsChecked = false;
            if (CancelJob.GetValue(RelativePanel.BelowProperty) == AppliedArea)
                CancelJob.SetValue(RelativePanel.BelowProperty, ActiveBox);
            DateAppliedBox.Date = null;
        }

        private void NotesBox_Checked(object sender, RoutedEventArgs e)
        {
            NotesBox.Visibility = Visibility.Visible;
            CancelJob.SetValue(RelativePanel.BelowProperty, NotesBox);
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

