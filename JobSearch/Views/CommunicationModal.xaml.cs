using JobSearch.Models;
using JobSearch.ViewModels;
using System;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JobSearch.Views
{
    public sealed partial class CommunicationModal : UserControl
    {
        private MainPageViewModel ViewModel;
        private Job_Communication Model;

        public CommunicationModal(Job_Communication model = null)
        {
            InitializeComponent();
            this.DataContext = ViewModel = MainPageViewModel.Instance;
            Model = model;

            if (Model != null)
            {
                TitleBlock.Text = "Edit Communication";
                FromBox.Text = Model.From;
                ToBox.Text = Model.To;
                SubjectBox.Text = Model.Subject;
                ViaBox.Text = Model.Via;
                DateBox.Date = Model.DateAndTime;
                TimeBox.Time = Model.DateAndTime?.TimeOfDay ?? default(TimeSpan);
                DescriptionBox.Text = Model.Description;
                //ResponseExpected.Text = Model.ResponseExpected;
            }
            else
                TitleBlock.Text = "Add Communication";
        }

        public void Okay_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Model == null)
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
                }
                else
                {
                    ViewModel.EditCommunication(
                        communicationId: Model.CommunicationId,
                        to: ToBox.Text,
                        from: FromBox.Text,
                        subject: SubjectBox.Text,
                        via: ViaBox.Text,
                        date: DateBox.Date?.Date,
                        time: TimeBox.Time,
                        description: DescriptionBox.Text
                    );
                }

                BootStrapper.Current.ModalDialog.IsModal = false;
            }
            catch (ArgumentNullException ex) { System.Diagnostics.Debug.Write(ex.Message); }
        }

        private void Cancel_Clicked(object sender, RoutedEventArgs e) => BootStrapper.Current.ModalDialog.IsModal = false;

    }
}

