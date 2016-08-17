using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using JobSearch.Controls;
using Template10.Common;
using Template10.Mvvm;
using System.Collections.Generic;
using JobSearch.Controls.ListViewItems;

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
                        (child as ListView).LayoutUpdated += (sender, e) => FindListViewItems(child);
                    }
                    FindListViews(child);
                }
            }
        }

        private void FindListViewItems(DependencyObject obj)
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is ListViewItem)
                    {
                        ListViewItem lvi = (child as ListViewItem);
                        if (lvi.ContentTemplateRoot is RequirementItem)
                            (lvi.ContentTemplateRoot as RequirementItem).ParentLVI = lvi;
                        else if (lvi.ContentTemplateRoot is ResponsibilityItem)
                            (lvi.ContentTemplateRoot as ResponsibilityItem).ParentLVI = lvi;
                        else if (lvi.ContentTemplateRoot is InterviewItem)
                            (lvi.ContentTemplateRoot as InterviewItem).ParentLVI = lvi;
                    }
                    else
                        FindListViewItems(child);
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
            BootStrapper.Current.ModalContent = new InterviewModal();
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
                    // allow multiple selection while keeping current job selection
                    object sel = JobsList.SelectedItem;
                    JobsList.SelectionMode = ListViewSelectionMode.Multiple;
                    foreach (ListView lv in ListViews)
                        lv.SelectionMode = ListViewSelectionMode.Multiple;
                    foreach (TextBlock tb in ItemBullets)
                        tb.Visibility = Visibility.Collapsed;
                    JobsList.SelectedItems.Add(sel);
                }
                else
                {
                    // allow single selection only while keeping first job selection
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

        private void JobSearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(JobSearch.Text))
                {
                    ViewModel.Filter(string.Empty);
                    JobSearch.ItemsSource = null;
                }
                else
                    JobSearch.ItemsSource = ViewModel.Search(JobSearch.Text);
            }
        }

        private void JobSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
            => ViewModel.Filter(JobSearch.Text);

        private void SortByPosition_Clicked(object sender, RoutedEventArgs e)
            => ViewModel.SortByPosition();

        private void SortByDateApplied_Clicked(object sender, RoutedEventArgs e)
            => ViewModel.SortByDateApplied();

        private void SortByDatePosted_Clicked(object sender, RoutedEventArgs e)
            => ViewModel.SortByDatePosted();

    }
}
