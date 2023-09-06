using System;
using System.Collections.Generic;
using Bouquet.Mobile.Intefaces;
using Bouquet.Mobile.Resources.Resx;
using Bouquet.Mobile.Services;
using Bouquet.Mobile.ViewModels;
using Bouquet.Mobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Bouquet.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(StartupPage), typeof(StartupPage));
            Routing.RegisterRoute(nameof(OrderDetailPage), typeof(OrderDetailPage));
            Routing.RegisterRoute(nameof(NewLandmarkPage), typeof(NewLandmarkPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
            Routing.RegisterRoute(nameof(CompletedOrders), typeof(CompletedOrders));

            CurrentItem = startupPage;
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            DependencyService.Resolve<ISignalRService>().StopSignalRConnection();
            Settings.Settings.LoggedUserId = "";
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
