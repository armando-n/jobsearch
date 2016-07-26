using Windows.UI.Xaml;
using System.Threading.Tasks;
using JobSearch.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using JobSearch.Views;
using System.IO;
using JobSearch.Models;

namespace JobSearch
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    //[Bindable]
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            //SplashFactory = (e) => new Views.Splash(e);
            Services.DatabaseService.DatabaseService.getDB();

            #region App settings

            //var _settings = SettingsService.Instance;
            //RequestedTheme = _settings.AppTheme;
            //CacheMaxDuration = _settings.CacheMaxDuration;
            //ShowShellBackButton = _settings.UseShellBackButton;

            #endregion
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
            return new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = new Views.Shell(service),
            };
        }

        //public override async Task OnInitializeAsync(IActivatedEventArgs args)
        //{
        //    if (Window.Current.Content as ModalDialog == null)
        //    {
        //        // create a new frame 
        //        var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);

        //        // create modal root
        //        Window.Current.Content = new ModalDialog
        //        {
        //            DisableBackButtonWhenModal = true,
        //            Content = new Views.Shell(nav),
        //            ModalContent = new Views.Busy(),
        //        };
        //    }
        //    await Task.CompletedTask;
        //}

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // long-running startup tasks go here
            //await Task.Delay(5000);

            NavigationService.Navigate(typeof(Views.MainPage));
            return Task.CompletedTask;
        }
    }
}

