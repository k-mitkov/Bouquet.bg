using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bouquet.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartupPage : ContentPage
    {

        CancellationTokenSource cts;

        public StartupPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Task.Run(async () => await GetCurrentLocation());
            base.OnAppearing();
            CheckLogin();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
        }

        private async Task CheckLogin()
        {
            
            if(string.IsNullOrEmpty(Settings.Settings.LoggedUserId))
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
        }

        private async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                Settings.Settings.Location = await Geolocation.GetLocationAsync(request, cts.Token);
                
            }
            catch (Exception ex)
            {
            }
        }
    }
}