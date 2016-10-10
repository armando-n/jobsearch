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

        public void SelectLast() => CompaniesList.SelectedIndex = ViewModel.CompanyCount() - 1;

        private void CompanySearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(CompanySearch.Text))
                {
                    ViewModel.Filter(string.Empty);
                    CompanySearch.ItemsSource = ViewModel.Search(string.Empty);
                }
                else
                    CompanySearch.ItemsSource = ViewModel.Search(CompanySearch.Text);
            }
        }

        private void CompanySearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                ViewModel.Filter(string.Empty);
                ViewModel.Select(args.ChosenSuggestion);
                CompaniesList.ScrollIntoView(args.ChosenSuggestion);
            }
            else
                ViewModel.Filter(CompanySearch.Text);
        }

        private void JobSelection_Changed(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
