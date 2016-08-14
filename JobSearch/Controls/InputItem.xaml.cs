using JobSearch.Models;
using JobSearch.ViewModels;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;

namespace JobSearch.Controls
{
    public sealed partial class InputItem : UserControl
    {
        public string Title { get; set; }
        public string MethodName { get; set; }
        public string InputName { get; set; }
        public string InitialText { get; set; }
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

        private void LVI_PointerEntered(object sender, PointerRoutedEventArgs e)
            => ToggleIcons(Visibility.Visible);

        private void LVI_PointerExited(object sender, PointerRoutedEventArgs e)
            => ToggleIcons(Visibility.Collapsed);

        public Job_Requirement JobRequirement
        {
            get { return (Job_Requirement)GetValue(JobRequirementProperty); }
            set { SetValueDp(JobRequirementProperty, value); }
        }
        public readonly DependencyProperty JobRequirementProperty
            = DependencyProperty.Register("JobRequirement", typeof(Job_Requirement), typeof(InputItem), null);

        private MainPageViewModel ViewModel;

        public InputItem()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
            //this.DataContext = ViewModel = MainPageViewModel.Instance;
            ViewModel = MainPageViewModel.Instance;
            this.Loaded += InitModal;
        }

        private void InitModal(object sender, RoutedEventArgs e)
        {
            
        }

        public void Okay_Clicked(object sender, RoutedEventArgs e)
        {
            //if (String.IsNullOrWhiteSpace(InputBox.Text))
            //    return;

            //try
            //{
            //    ViewModel.GetType().GetMethod(MethodName).Invoke(ViewModel, new[] { InputBox.Text });
            //    BootStrapper.Current.ModalDialog.IsModal = false;
            //}
            //catch (ArgumentNullException ex) { System.Diagnostics.Debug.Write(ex.Message); }
        }

        private void Cancel_Clicked(object sender, RoutedEventArgs e) => BootStrapper.Current.ModalDialog.IsModal = false;

        private void Icon_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            (((sender as Border).Child as Viewbox).Child as SymbolIcon).Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
        }

        private void Icon_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            (((sender as Border).Child as Viewbox).Child as SymbolIcon).Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
        }

        private void EditRequirement_Clicked(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            BootStrapper.Current.ModalContent = new InputModal
            {
                Title = "Edit Requirement",
                MethodName = "EditRequirement",
                Target = this,
                InputName = "Requirement",
                InitialText = JobRequirement.Requirement
            };
            BootStrapper.Current.ModalDialog.IsModal = true;
        }

        public void EditRequirement(string newRequirement)
            => ViewModel.EditRequirement(newRequirement, JobRequirement.RequirementId);

        private void DeleteRequirement_Clicked(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
            => ViewModel.DeleteRequirement(JobRequirement.RequirementId);

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

