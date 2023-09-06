using System.ComponentModel;
using Bouquet.Mobile.ViewModels;
using Xamarin.Forms;

namespace Bouquet.Mobile.Views
{
    public partial class OrderDetailPage : ContentPage
    {
        public OrderDetailPage()
        {
            InitializeComponent();
            BindingContext = new OrderDetailViewModel();
        }
    }
}