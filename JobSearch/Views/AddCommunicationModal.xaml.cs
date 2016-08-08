using JobSearch.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JobSearch.Views
{
    public sealed partial class AddCommunicationModal : UserControl
    {
        private MainPageViewModel ViewModel;

        public AddCommunicationModal()
        {
            InitializeComponent();
            this.DataContext = ViewModel = MainPageViewModel.Instance;
        }

        public void Okay_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AddCommunication(
                    to: ToBox.Text,
                    from: FromBox.Text,
                    subject: SubjectBox.Text,
                    via: ViaBox.Text,
                    date: DateBox.Date?.Date,
                    time: TimeBox.Time,
                    description: DescriptionBox.Text
                );
                BootStrapper.Current.ModalDialog.IsModal = false;
            }
            catch (ArgumentNullException ex) { System.Diagnostics.Debug.Write(ex.Message); }
        }

        private void Cancel_Clicked(object sender, RoutedEventArgs e) => BootStrapper.Current.ModalDialog.IsModal = false;

    }
}

