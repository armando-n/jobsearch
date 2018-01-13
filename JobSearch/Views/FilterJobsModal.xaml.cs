using JobSearch.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JobSearch.Views
{
    public sealed partial class FilterJobsModal : UserControl
    {
        private MainPageViewModel ViewModel;

        public FilterJobsModal()
        {
            InitializeComponent();
            this.DataContext = ViewModel = MainPageViewModel.Instance;
        }

        private void CloseModal(object sender, RoutedEventArgs e)
        {
            BootStrapper.Current.ModalDialog.IsModal = false;
        }

    }
}

