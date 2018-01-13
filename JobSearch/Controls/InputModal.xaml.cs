using JobSearch.ViewModels;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JobSearch.Controls
{
    public sealed partial class InputModal : UserControl
    {
        public string Title { get; set; }
        public string MethodName { get; set; }
        public string MessageText { get; set; }
        public string InputName { get; set; }
        public string InitialText { get; set; }
        public object Target { get; internal set; }

        private MainPageViewModel ViewModel;

        public InputModal()
        {
            InitializeComponent();
            this.DataContext = ViewModel = MainPageViewModel.Instance;
            this.Loaded += InitModal;
        }

        private void InitModal(object sender, RoutedEventArgs e)
        {
            TitleBlock.Text = Title ?? "Input";
            MessageBlock.Text = MessageText ?? "";
            InputBox.Header = InputName ?? "";
            InputBox.Text = InitialText ?? "";
            if (!String.IsNullOrWhiteSpace(MessageBlock.Text))
                MessageBlock.Visibility = Visibility.Visible;
        }

        public void Okay_Clicked(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(InputBox.Text))
                return;

            try
            {
                if (Target != null)
                    Target.GetType().GetMethod(MethodName).Invoke(Target, new[] { InputBox.Text });
                else
                    ViewModel.GetType().GetMethod(MethodName).Invoke(ViewModel, new[] { InputBox.Text });
                BootStrapper.Current.ModalDialog.IsModal = false;
            }
            catch (ArgumentNullException ex) { System.Diagnostics.Debug.Write(ex.Message); }
        }

        private void Cancel_Clicked(object sender, RoutedEventArgs e) => BootStrapper.Current.ModalDialog.IsModal = false;

    }
}

