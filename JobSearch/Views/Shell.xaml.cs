using System.ComponentModel;
using System.Linq;
using System;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            {
                AddSymbol.Symbol = (AddSymbol.Symbol == Symbol.Add) ? Symbol.Remove : Symbol.Add;
                MainPage.Instance.ToggleAddJobForm();
            }
            else if (currentPage.Equals(typeof(CompaniesPage)))
            {
                AddSymbol.Symbol = (AddSymbol.Symbol == Symbol.Add) ? Symbol.Remove : Symbol.Add;
                CompaniesPage.Instance.ToggleAddCompanyForm();
            }

            else if (currentPage.Equals(typeof(RecruitersPage)))
            {
                AddSymbol.Symbol = (AddSymbol.Symbol == Symbol.Add) ? Symbol.Remove : Symbol.Add;
                RecruitersPage.Instance.ToggleAddRecruiterForm();
            }
            
        }

        private void NavButton_Tapped(object sender, RoutedEventArgs e)
        {
            AddSymbol.Symbol = Symbol.Add;
        }
    }
}

