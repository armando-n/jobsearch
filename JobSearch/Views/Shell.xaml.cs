using System.ComponentModel;
using System.Linq;
using System;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Template10.Common;

namespace JobSearch.Views
{
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;

        public Shell()
        {
            Instance = this;
            InitializeComponent();
        }

        public Shell(INavigationService navigationService) : this()
        {
            SetNavigationService(navigationService);
        }

        public void SetNavigationService(INavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
        }

        private void Add_Clicked(object sender, RoutedEventArgs e)
        {
            Type currentPage = MyHamburgerMenu.NavigationService.Content.GetType();

            if (currentPage.Equals(typeof(MainPage)))
                BootStrapper.Current.ModalContent = new AddJobModal();
            else if (currentPage.Equals(typeof(CompaniesPage)))
                BootStrapper.Current.ModalContent = new AddCompanyModal();
            else if (currentPage.Equals(typeof(RecruitersPage)))
                BootStrapper.Current.ModalContent = new AddRecruiterModal();

            BootStrapper.Current.ModalDialog.IsModal = true;
        }

    }
}

