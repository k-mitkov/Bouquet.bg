using Android.App;
using Android.Widget;
using Bouquet.Mobile.Droid;
using Bouquet.Mobile.Intefaces;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace Bouquet.Mobile.Droid
{
    public class MessageAndroid : IMessage
    {
        private Toast toast;

        public void LongAlert(string message)
        {
            toast?.Cancel();

            toast = Toast.MakeText(Application.Context, message, ToastLength.Long);

            toast.Show();
        }

        public void ShortAlert(string message)
        {
            toast?.Cancel();

            toast = Toast.MakeText(Application.Context, message, ToastLength.Short);

            toast.Show();
        }
    }
}