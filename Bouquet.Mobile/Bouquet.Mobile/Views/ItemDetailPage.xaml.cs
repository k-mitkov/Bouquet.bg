using Bouquet.Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Bouquet.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}