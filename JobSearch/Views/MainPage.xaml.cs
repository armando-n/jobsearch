using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using JobSearch.Controls;
using Template10.Common;

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

        private void ShowRequirementsModal(object sender, RoutedEventArgs e)
        {
            BootStrapper.Current.ModalContent = new InputModal
            {
                Title = "Add Requirement",
                MethodName = "AddRequirement",
                InputName = "Requirement"
            };
            BootStrapper.Current.ModalDialog.IsModal = true;
        }

        private void ShowResponsibilitiesModal(object sender, RoutedEventArgs e)
        {
            BootStrapper.Current.ModalContent = new InputModal
            {
                Title = "Add Responsibility",
                MethodName = "AddResponsibility",
                InputName = "Responsibility"
            };
            BootStrapper.Current.ModalDialog.IsModal = true;
        }

        private void ShowNotesModal(object sender, RoutedEventArgs e)
        {
            BootStrapper.Current.ModalContent = new InputModal
            {
                Title = "Notes",
                MethodName = "SetNotes",
                InputName = "Notes",
                InitialText = (ViewModel.Selected as Models.Job).Notes
            };
            BootStrapper.Current.ModalDialog.IsModal = true;
        }

        private void ShowTestModal(object sender, RoutedEventArgs e)
        {
            BootStrapper.Current.ModalContent = new AddTestModal();
            BootStrapper.Current.ModalDialog.IsModal = true;
        }

        private void ShowInterviewModal(object sender, RoutedEventArgs e)
        {
            BootStrapper.Current.ModalContent = new AddInterviewModal();
            BootStrapper.Current.ModalDialog.IsModal = true;
        }

        private void ShowCommunicationModal(object sender, RoutedEventArgs e)
        {
            BootStrapper.Current.ModalContent = new AddCommunicationModal();
            BootStrapper.Current.ModalDialog.IsModal = true;
        }
    }
}
