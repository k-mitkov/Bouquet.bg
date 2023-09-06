using System;
using System.Collections.Generic;
using System.ComponentModel;
using Bouquet.Mobile.Models;
using Bouquet.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bouquet.Mobile.Views
{
    public partial class NewLandmarkPage : ContentPage
    {
        public Item Item { get; set; }

        public NewLandmarkPage()
        {
            InitializeComponent();
            BindingContext = new NewLandmarkViewModel();
        }
    }
}