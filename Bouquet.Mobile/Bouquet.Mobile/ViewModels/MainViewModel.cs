using Bouquet.Mobile.Enums;
using Bouquet.Mobile.Helpers;
using Bouquet.Mobile.Intefaces;
using Bouquet.Mobile.Models;
using Bouquet.Mobile.Resources.Resx;
using Bouquet.Mobile.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Bouquet.Mobile.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Declarations

        private OrderDTO selectedOrder;

        ///// <summary>
        ///// Команда при показване на екрана
        /// </summary>
        private ICommand onAppearingCommand;

        ///// <summary>
        ///// Команда при показване на екрана
        /// </summary>
        private ICommand onDisappearinggCommand;

        List<OrderDTO> orders;

        #endregion

        #region Properties

        public OrderDTO SelectedOrder
        {
            get
            {
                return selectedOrder;
            }
            set
            {
                selectedOrder = null;
                OnPropertyChanged();

                OnItemSelected(value);
            }
        }

        public List<OrderDTO> Orders
        {
            get
            {
                return orders;
            }
            set
            {
                orders = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Команда при показване на екрана
        /// </summary>
        public ICommand OnAppearingCommand => onAppearingCommand ?? (onAppearingCommand = new Command(OnAppearing));

        #endregion

        #region Constructors

        public MainViewModel()
        {
            
        }

        #endregion

        #region Methods

        private async void OnItemSelected(OrderDTO item)
        {
            if (item == null)
                return;

            var serializedItem = JsonConvert.SerializeObject(item);

            await Shell.Current.GoToAsync($"{nameof(OrderDetailPage)}?{nameof(OrderDetailViewModel.Item)}={serializedItem}");
        }


        /// <summary>
        /// Презарежда потребителите при всяко отваряне на екрана
        /// </summary>
        /// <param name="obj"></param>
        private void OnAppearing(object obj)
        {
            IsBusy = true;
            Task.Run(async () => await GetOrders());
        }

        private async Task GetOrders()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/api/order?shopID=" + Settings.Settings.SelectedShopId);
                var response = await ApiRequestHandler.Instance.SendAuthenticatedRequestAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsAsync<Response<IEnumerable<OrderDTO>>>();

                    if (responseData.Status == StatusEnum.Success)
                    {
                        var orders = responseData.Data;

                        Orders = orders.Where(o => o.Status != 4).ToList();
                        //Orders.Sort((x, y) => x.Distance.CompareTo(y.Distance));
                        IsBusy = false;
                    }
                    else
                    {
                        IsBusy = false;
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService.Resolve<IMessage>().LongAlert(AppResources.strNoConnection);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Resolve<IMessage>().LongAlert(AppResources.strNoConnection);
                });
            }
        }

        #endregion
    }
}
