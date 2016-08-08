using JobSearch.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JobSearch.Views
{
    public sealed partial class AddTestModal : UserControl
    {
        private MainPageViewModel ViewModel;

        public AddTestModal()
        {
            InitializeComponent();
            this.DataContext = ViewModel = MainPageViewModel.Instance;
        }

        public void Okay_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AddTest(
                    type: TypeBox.Text,
                    date: DateBox.Date?.Date,
                    time: TimeBox.Time,
                    notes: NotesBox.Text
                );
                BootStrapper.Current.ModalDialog.IsModal = false;
            }
            catch (ArgumentNullException ex) { System.Diagnostics.Debug.Write(ex.Message); }
        }

        private void Cancel_Clicked(object sender, RoutedEventArgs e) => BootStrapper.Current.ModalDialog.IsModal = false;

    }
}

