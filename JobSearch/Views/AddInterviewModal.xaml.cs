using JobSearch.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JobSearch.Views
{
    public sealed partial class AddInterviewModal : UserControl
    {
        private MainPageViewModel ViewModel;

        public AddInterviewModal()
        {
            InitializeComponent();
            this.DataContext = ViewModel = MainPageViewModel.Instance;
        }

        public void Okay_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AddInterview(
                    via: ViaBox.Text,
                    interviewer: InterviewerBox.Text,
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

