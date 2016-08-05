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

        public void ToggleAddRecruiterForm()
        {
            AddRecruiterForm.Visibility = (AddRecruiterForm.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void AddRecruiter_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AddRecruiter(
                    name: NameBox.Text,
                    email: EmailBox.Text,
                    title: TitleBox.Text,
                    notes: null
                );

                AddRecruiterForm.Visibility = Visibility.Collapsed;
                RecruitersList.SelectedIndex = ViewModel.RecruiterCount() - 1;
            }
            catch (ArgumentNullException ex)
            {

            }
        }

        private void FillForm_Clicked(object sender, RoutedEventArgs e)
        {
            NameBox.Text = "Robin Sherbatzky";
            EmailBox.Text = "robin@himym.com";
            TitleBox.Text = "Queen Bee";
        }
    }
}
