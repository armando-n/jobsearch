using JobSearch.Models;
using JobSearch.ViewModels;
using System;
using System.ComponentModel;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using JobSearch.Views;
using Windows.UI.Popups;

namespace JobSearch.Controls.ListViewItems
{
    public sealed partial class CommunicationItem : UserControl
    {
        private const int AdaptiveBoundaryWidth = 350;
        private MainPageViewModel ViewModel;

        private ListViewItem _parentLVI;
        public ListViewItem ParentLVI
        {
            get { return _parentLVI; }
            set
            {
                if (_parentLVI != value)
                {
                    _parentLVI = value;
                    _parentLVI.PointerEntered += LVI_PointerEntered;
                    _parentLVI.PointerExited += LVI_PointerExited;
                    _parentLVI.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    _parentLVI.VerticalContentAlignment = VerticalAlignment.Stretch;
                    _parentLVI.Padding = new Thickness(0);
                }
            }
        }

        public Job_Communication Model
        {
            get { return (Job_Communication)GetValue(ModelProperty); }
            set { SetValueDp(ModelProperty, value); }
        }
        public readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(Job_Communication), typeof(CommunicationItem), null);

        public CommunicationItem()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
            ViewModel = MainPageViewModel.Instance;
            this.SizeChanged += UpdateLayout;
        }

        private void UpdateLayout(object sender, SizeChangedEventArgs e)
        {
            if (TopPanel.ActualWidth < AdaptiveBoundaryWidth) // TODO Make this check the current visual state as well
                VisualStateManager.GoToState(this, "NarrowVisualState", false);
            else
                VisualStateManager.GoToState(this, "NormalVisualState", false);
        }

        private void Icon_PointerEntered(object sender, PointerRoutedEventArgs e)
            => (((sender as Border).Child as Viewbox).Child as SymbolIcon).Foreground = new SolidColorBrush(Windows.UI.Colors.Red);

        private void Icon_PointerExited(object sender, PointerRoutedEventArgs e)
            => (((sender as Border).Child as Viewbox).Child as SymbolIcon).Foreground = new SolidColorBrush(Windows.UI.Colors.Black);

        private void Edit_Clicked(object sender, PointerRoutedEventArgs e)
        {
            BootStrapper.Current.ModalContent = new CommunicationModal(Model);
            BootStrapper.Current.ModalDialog.IsModal = true;
        }

        private void Delete_Clicked(object sender, PointerRoutedEventArgs e)
            => ShowDeleteConfirmationDialog();

        private async void ShowDeleteConfirmationDialog()
        {
            var dialog = new MessageDialog("Are you sure you want to delete this communication?");
            dialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(this.ConfirmationHandler)));
            dialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler(this.ConfirmationHandler)));
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            await dialog.ShowAsync();
        }

        private void ConfirmationHandler(IUICommand command)
        {
            if (command.Label == "Yes")
                ViewModel.DeleteCommunication(Model.CommunicationId);
        }

        private void LVI_PointerEntered(object sender, PointerRoutedEventArgs e) => ToggleIcons(Visibility.Visible);
        private void LVI_PointerExited(object sender, PointerRoutedEventArgs e) => ToggleIcons(Visibility.Collapsed);
        public void ToggleIcons(Visibility visibility)
        {
            EditIcon.Visibility = visibility;
            DeleteIcon.Visibility = visibility;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void SetValueDp(DependencyProperty property, object value,
                [System.Runtime.CompilerServices.CallerMemberName] String p = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

    }
}

