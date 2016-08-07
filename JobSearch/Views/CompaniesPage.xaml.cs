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

    }
}
