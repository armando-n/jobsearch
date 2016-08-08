using JobSearch.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JobSearch.Views
{
    public sealed partial class AddRecruiterModal : UserControl
    {
        private RecruitersPageViewModel ViewModel;

        public AddRecruiterModal()
        {
            InitializeComponent();
            this.DataContext = ViewModel = RecruitersPageViewModel.Instance;
        }

        public void AddRecruiter_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AddRecruiter(
                    name: NameBox.Text,
                    email: EmailBox.Text,
                    title: TitleBox.Text,
                    notes: NotesBox.Text
                );

                BootStrapper.Current.ModalDialog.IsModal = false;
                CompaniesPage.Instance.SelectLast();
            }
            catch (ArgumentNullException ex) { }
        }

        private void CancelRecruiter_Clicked(object sender, RoutedEventArgs e) => BootStrapper.Current.ModalDialog.IsModal = false;

    }
}

