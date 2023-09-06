using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Bouquet.Mobile.Droid;
using Bouquet.Mobile.Intefaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(SignalRService))]
namespace Bouquet.Mobile.Droid
{
    [Service]
    public class SignalRService : Service, ISignalRService
    {
        private bool _isRunning;
        private HubConnection _hubConnection; // Import the appropriate SignalR classes

        private PowerManager.WakeLock _wakeLock;
        private const string WakeLockTag = "SignalRServiceWakeLock";

        private string email = "";
        private string message = "";

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (!_isRunning)
            {
                AcquireWakeLock();
                StartForegroundService();
                _isRunning = true;
            }

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            //StopForegroundService();
            StopSignalRConnection();
            ReleaseWakeLock();
            _isRunning = false;
            base.OnDestroy();
        }

        public void StartSignalRConnection(string email, string TranslatedMessage)
        {
            Task.Run(async () =>
            {
                try
                {
                    this.email = email;
                    this.message = TranslatedMessage;
                    Connectivity.ConnectivityChanged += ConnectivityChanged;

                    _hubConnection = new HubConnectionBuilder()
                        .WithUrl(AppConstands.SignalRURL + $"/mobilehub?clientId={email}", (opts) =>
                        {
                            opts.HttpMessageHandlerFactory = (message) =>
                            {
                                if (message is HttpClientHandler clientHandler)
                                    // bypass SSL certificate
                                    clientHandler.ServerCertificateCustomValidationCallback +=
                                        (sender, certificate, chain, sslPolicyErrors) => { return true; };
                                return message;
                            };
                            opts.Headers["ConnectionId"] = email;
                        }).Build();

                    _hubConnection.On<string>("ReceiveMessage", (message) =>
                    {
                        HandleIncomingMessage(TranslatedMessage);
                    });

                    await _hubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    var a = ex;
                }

            });
        }

        public void StopSignalRConnection()
        {
            Task.Run(async () =>
            {
                try
                {
                    Connectivity.ConnectivityChanged -= ConnectivityChanged;


                    await _hubConnection.StopAsync();
                }
                catch { }
            });
        }

        private void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.None)
                StartSignalRConnection(email, message);
        }

        private void AcquireWakeLock()
        {
            var powerManager = (PowerManager)GetSystemService(PowerService);
            _wakeLock = powerManager.NewWakeLock(WakeLockFlags.Partial, WakeLockTag);
            _wakeLock.Acquire();
        }

        private void ReleaseWakeLock()
        {
            if (_wakeLock?.IsHeld == true)
            {
                _wakeLock.Release();
            }
        }

        private void StartForegroundService()
        {
            var intent = new Intent(this, typeof(MainActivity)); // Replace with your main activity
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);

            var notification = new NotificationCompat.Builder(this, "default")
                .SetSmallIcon(Resource.Drawable.logo)
                .SetContentTitle("SignalR Service")
                .SetContentText("Running")
                .SetContentIntent(pendingIntent)
                .SetOngoing(true)
                .Build();

            StartForeground(1, notification);
        }

        private void HandleIncomingMessage(string message)
        {
            var intent = new Intent(MainActivity.Instance, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            var pendingIntent = PendingIntent.GetActivity(MainActivity.Instance, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(MainActivity.Instance, "default")
                .SetSmallIcon(Resource.Drawable.logo)
                .SetContentTitle("New Message")
                .SetContentText(message)
                .SetPriority(NotificationCompat.PriorityHigh)
                .SetDefaults(NotificationCompat.DefaultAll)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(MainActivity.Instance);
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}