using JobSearch.Models;
using JobSearch.ViewModels;
using System;
using System.ComponentModel;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;

namespace JobSearch.Controls.ListViewItems
{
    public sealed partial class ResponsibilityItem : UserControl
    {
        public string Title { get; set; }
        public string MethodName { get; set; }
        public string InputName { get; set; }
        public string InitialText { get; set; }
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

        public Job_Responsibility Model
        {
            get { return (Job_Responsibility)GetValue(ModelProperty); }
            set { SetValueDp(ModelProperty, value); }
        }
        public readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(Job_Responsibility), typeof(ResponsibilityItem), null);

        public ResponsibilityItem()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
            ViewModel = MainPageViewModel.Instance;
        }

        private void Icon_PointerEntered(object sender, PointerRoutedEventArgs e)
            => (((sender as Border).Child as Viewbox).Child as SymbolIcon).Foreground = new SolidColorBrush(Windows.UI.Colors.Red);

        private void Icon_PointerExited(object sender, PointerRoutedEventArgs e)
            => (((sender as Border).Child as Viewbox).Child as SymbolIcon).Foreground = new SolidColorBrush(Windows.UI.Colors.Black);

        private void Edit_Clicked(object sender, PointerRoutedEventArgs e)
        {
            BootStrapper.Current.ModalContent = new InputModal
            {
                Title = "Edit Responsibility",
                MethodName = "Okay_Clicked",
                Target = this,
                InputName = "Responsibility",
                InitialText = Model.Responsibility
            };
            BootStrapper.Current.ModalDialog.IsModal = true;
        }

        public void Okay_Clicked(string newInput)
            => ViewModel.EditResponsibility(newInput, Model.ResponsibilityId);

        private void Delete_Clicked(object sender, PointerRoutedEventArgs e)
            => ViewModel.DeleteResponsibility(Model.ResponsibilityId);

        public void ToggleIcons(Visibility visibility)
        {
            EditIcon.Visibility = visibility;
            DeleteIcon.Visibility = visibility;
        }

        private void LVI_PointerEntered(object sender, PointerRoutedEventArgs e) => ToggleIcons(Visibility.Visible);
        private void LVI_PointerExited(object sender, PointerRoutedEventArgs e) => ToggleIcons(Visibility.Collapsed);

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

