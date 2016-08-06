using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using JobSearch.Controls;

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

        public void SelectLast() => JobsList.SelectedIndex = ViewModel.JobCount() - 1;

        private void ShowTestPopup(object sender, RoutedEventArgs e)
        {
            Popup popup = FindSiblingPopup(sender);
            popup.IsOpen = true;
            (popup.Child as TestForm).Focus();
        }

        private void ShowInterviewPopup(object sender, RoutedEventArgs e)
        {
            Popup popup = FindSiblingPopup(sender);
            popup.IsOpen = true;
            (popup.Child as InterviewForm).Focus();
        }

        private void ShowCommunicationPopup(object sender, RoutedEventArgs e)
        {
            Popup popup = FindSiblingPopup(sender);
            popup.IsOpen = true;
            (popup.Child as CommunicationForm).Focus();
        }

        private Popup FindSiblingPopup(object sender)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(sender as DependencyObject);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is Popup)
                    return child as Popup;
            }

            return null;
        }
    }
}
