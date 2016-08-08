using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JobSearch.Views
{
    public sealed partial class RecruitersPage : Page
    {
        public static RecruitersPage Instance { get; private set; }

        public RecruitersPage()
        {
            InitializeComponent();
            //NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Instance = this;
        }

        public void SelectLast() => RecruitersList.SelectedIndex = ViewModel.RecruiterCount() - 1;

    }
}
