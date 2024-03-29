using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using JobSearch.Controls;
using Template10.Common;
using Template10.Mvvm;
using System.Collections.Generic;
using JobSearch.Controls.ListViewItems;
using JobSearch.ViewModels;
using Windows.UI;
using Windows.UI.Popups;

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
            this.Loaded += (sender, e) => FindListViews(contentControl);
        }

        private void SelectedUpdated(object sender, RoutedEventArgs e)
            => ViewModel.UpdateSelected();

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
                        else if (lvi.ContentTemplateRoot is CommunicationItem)
                            (lvi.ContentTemplateRoot as CommunicationItem).ParentLVI = lvi;
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

        public void SelectFirst() => JobsList.SelectedIndex = 0;

        private void ShowRequirementsModal(object sender, RoutedEventArgs e)
        {
            BootStrapper.Current.ModalContent = new InputModal
            {
                Title = "Add Requirement",
                MethodName = "AddRequirement",
                MessageText = "You can specify multiple requirements by pressing Enter to start another requirement on a new line."
            };
            BootStrapper.Current.ModalDialog.IsModal = true;
        }

        private void ShowResponsibilitiesModal(object sender, RoutedEventArgs e)
        {
            BootStrapper.Current.ModalContent = new InputModal
            {
                Title = "Add Responsibility",
                MethodName = "AddResponsibility",
                MessageText = "You can specify multiple responsibilities by pressing Enter to start another responsibility on a new line."
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
            BootStrapper.Current.ModalContent = new CommunicationModal();
            BootStrapper.Current.ModalDialog.IsModal = true;
        }

        private DelegateCommand _deleteSelection;
        public DelegateCommand DeleteSelection
            => _deleteSelection ?? (_deleteSelection = new DelegateCommand(
                () => ShowDeleteConfirmationDialog(), () => true));

        private async void ShowDeleteConfirmationDialog()
        {
            var dialog = new MessageDialog("Are you sure you want to delete the selected job?");
            dialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(this.ConfirmationHandler)));
            dialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler(this.ConfirmationHandler)));
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            await dialog.ShowAsync();
        }

        private void ConfirmationHandler(IUICommand command)
        {
            if (command.Label == "Yes")
                ViewModel.DeleteSelectedJob();
        }

        private DelegateCommand _flagSelection;
        public DelegateCommand FlagSelection
            => _flagSelection ?? (_flagSelection = new DelegateCommand(() =>
            {
                if (FlagButton.Label.Equals("Flag Job"))
                {
                    ViewModel.FlagSelection(true);
                    SetFlagButton(false);
                }
                else
                {
                    ViewModel.FlagSelection(false);
                    SetFlagButton(true);
                }
            }, () => true));

        private void SetFlagButton(bool? flagged)
        {
            if (flagged ?? true)
            {
                FlagButton.Foreground = new SolidColorBrush(Colors.Black);
                FlagButton.Label = "Flag Job";
                ToolTipService.SetToolTip(FlagButton, "Flag Selected Job");
            }
            else
            {
                FlagButton.Foreground = new SolidColorBrush(Colors.Red);
                FlagButton.Label = "Unflag Job";
                ToolTipService.SetToolTip(FlagButton, "Unflag Selected Job");
            }
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

            SetFlagButton(! ViewModel.IsSelectionFlagged());

            if (ViewModel.Selected == null)
            {
                DeleteButton.Visibility = Visibility.Collapsed;
                FlagButton.Visibility = Visibility.Collapsed;
            }   
            else
            {
                DeleteButton.Visibility = Visibility.Visible;
                FlagButton.Visibility = Visibility.Visible;
            }
        }

        private void JobSearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(JobSearch.Text))
                {
                    ViewModel.FilterJobs(string.Empty);
                    JobSearch.ItemsSource = null;
                }
                else
                    JobSearch.ItemsSource = ViewModel.SearchJobs(JobSearch.Text);
            }
        }

        private void JobSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
                ViewModel.FilterJobs((args.ChosenSuggestion as SearchResult).Result);
            else
                ViewModel.FilterJobs(JobSearch.Text);
        }

        private void SortBy_Clicked(object sender, RoutedEventArgs e)
            => ViewModel.SortBy((sender as MenuFlyoutItem).Text);

    }
}
