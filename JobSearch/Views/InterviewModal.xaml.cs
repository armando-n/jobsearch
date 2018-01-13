using JobSearch.Models;
using JobSearch.ViewModels;
using System;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JobSearch.Views
{
    public sealed partial class InterviewModal : UserControl
    {
        private MainPageViewModel ViewModel;
        private Job_Interview Model;

        public InterviewModal(Job_Interview model = null)
        {
            InitializeComponent();
            this.DataContext = ViewModel = MainPageViewModel.Instance;
            Model = model;

            if (Model != null)
            {
                TitleBlock.Text = "Edit Interview";
                ViaBox.Text = Model.Via;
                InterviewerBox.Text = Model.Interviewer;
                DateBox.Date = Model.DateAndTime;
                TimeBox.Time = Model.DateAndTime?.TimeOfDay ?? default(TimeSpan);
                NotesBox.Text = Model.Notes;
            }
            else
                TitleBlock.Text = "Add Interview";
        }

        public void Okay_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Model == null)
                {
                    ViewModel.AddInterview(
                        via: ViaBox.Text,
                        interviewer: InterviewerBox.Text,
                        date: DateBox.Date?.Date,
                        time: TimeBox.Time,
                        notes: NotesBox.Text
                    );
                }
                else
                {
                    ViewModel.EditInterview(
                        interviewId: Model.InterviewId,
                        via: ViaBox.Text,
                        interviewer: InterviewerBox.Text,
                        date: DateBox.Date?.Date,
                        time: TimeBox.Time,
                        notes: NotesBox.Text
                    );
                }

                BootStrapper.Current.ModalDialog.IsModal = false;
            }
            catch (ArgumentNullException ex) { System.Diagnostics.Debug.Write(ex.Message); }
        }

        private void Cancel_Clicked(object sender, RoutedEventArgs e) => BootStrapper.Current.ModalDialog.IsModal = false;

    }
}

