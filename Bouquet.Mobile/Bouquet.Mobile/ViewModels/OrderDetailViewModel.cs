using Bouquet.Mobile.Enums;
using Bouquet.Mobile.Helpers;
using Bouquet.Mobile.Models;
using Bouquet.Mobile.Resources.Resx;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Bouquet.Mobile.Intefaces;
using Bouquet.Mobile.Views;

namespace Bouquet.Mobile.ViewModels
{
    [QueryProperty(nameof(Item), nameof(Item))]
    public class OrderDetailViewModel : BaseViewModel
    {
        private string item;
        OrderDTO order;
        private int newStatus = 3;
        private ICommand changeStatusCommand;

        public OrderDetailViewModel()
        {
        }

        public ICommand ChangeStatusCommand => changeStatusCommand ?? (changeStatusCommand = new Command(async () => await ChangeStatus()));

        public string Id { get; set; }

        public string ButtonText
        {
            get
            {
                if(Order == null)
                {
                    return "";
                }

                switch (Order.Status) {
                    case 1:
                        return AppResources.strAccept;
                    case 2:
                        return AppResources.strAccept;
                    case 3:
                        newStatus = 4;
                        return AppResources.strComplete;
                    default:
                        return AppResources.strUnknown;
                }

            }
        }

        public string Item
        {
            set
            {
                item = value;
                GetOrder(item);
            }
        }

        public OrderDTO Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ButtonText));
            }
        }


        private void GetOrder(string item)
        {
            Order = JsonConvert.DeserializeObject<OrderDTO>(item);
        }

        private async Task ChangeStatus()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/api/order/update-status?orderID=" + Order.Id + "&status=" + newStatus);
                var response = await ApiRequestHandler.Instance.SendAuthenticatedRequestAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsAsync<Response<IEnumerable<OrderDTO>>>();

                    if (responseData.Status == StatusEnum.Success)
                    {
                        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
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
    }
}
