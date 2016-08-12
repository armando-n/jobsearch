using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using JobSearch.Controls;
using Template10.Common;
using Template10.Mvvm;
using System.Collections.Generic;

namespace JobSearch.Views
{
    public sealed partial class MainPage : Page
    {
        public static MainPage Instance { get; private set; }
        public List<TextBlock> ItemBullets { get; set; }
        public List<ListView> ListViews { get; set; }

        public MainPage()
        {
            InitializeComponent();
            //NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Instance = this;
            ListViews = new List<ListView>();
            ItemBullets = new List<TextBlock>();
        }

        private void FindListViews(DependencyObject obj)
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is ListView)
                    {
                        ListViews.Add(child as ListView);
                        FindChildBullet(child);
                    }
                    FindListViews(child);
                }
            }
        }

        private void FindChildBullet(DependencyObject obj)
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is TextBlock && (child as TextBlock).Text == "\u2022")
                    {
                        ItemBullets.Add(child as TextBlock);
                        return;
                    }
                    FindChildBullet(child);
                }
            }
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

        private DelegateCommand _enterSelectionMode;
        public DelegateCommand EnterSelectionMode
            => _enterSelectionMode ?? (_enterSelectionMode = new DelegateCommand(() =>
            {
                if (JobsList.SelectionMode == ListViewSelectionMode.Single)
                {
                    object sel = JobsList.SelectedItem;
                    JobsList.SelectionMode = ListViewSelectionMode.Multiple;
                    JobsList.SelectedItems.Add(sel);
                    foreach (ListView lv in ListViews)
                        lv.SelectionMode = ListViewSelectionMode.Multiple;
                    foreach (TextBlock tb in ItemBullets)
                        tb.Visibility = Visibility.Collapsed;
                }
                else
                {
                    object sel = null;
                    if (JobsList.SelectedItems.Count > 0)
                    {
                        IEnumerator<object> selEnum = JobsList.SelectedItems.GetEnumerator();
                        selEnum.MoveNext();
                        sel = selEnum.Current;
                    }
                    JobsList.SelectionMode = ListViewSelectionMode.Single;
                    JobsList.SelectedItem = sel;
                    foreach (ListView lv in ListViews)
                        lv.SelectionMode = ListViewSelectionMode.Single;
                    foreach (TextBlock tb in ItemBullets)
                        tb.Visibility = Visibility.Visible;
                }
            }, () => true));

        private void JobSelection_Changed(object sender, SelectionChangedEventArgs e)
        {
            ListViews.Clear();
            ItemBullets.Clear();
            FindListViews(contentControl);
        }
    }
}
