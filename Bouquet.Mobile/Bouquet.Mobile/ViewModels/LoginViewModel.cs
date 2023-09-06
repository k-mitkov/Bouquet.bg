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
using System.Text;
using System.Windows.Input;
using WarehouseMobile.Commands;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Bouquet.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        #region Declarations

        /// <summary>
        /// избрания потребител
        /// </summary>
        private LoginModel selectedUser;

        /// <summary>
        /// въведената парола
        /// </summary>
        private string password;

        /// <summary>
        /// въведената парола
        /// </summary>
        private string username;

        /// <summary>
        /// Потребителите в базата
        /// </summary>
        private IList<LoginModel> users;

        /// <summary>
        /// Команда за вход
        /// </summary>
        private ICommand loginCommand;

        private ICommand registerCommand;

        /// <summary>
        /// Команда при показване на екрана
        /// </summary>
        private ICommand onAppearingCommand;

        #endregion

        #region Constructors

        public LoginViewModel()
        {
        }

        #endregion

        #region Properties

        #region Commands

        /// <summary>
        /// Команда за вход
        /// </summary>
        public ICommand LoginCommand => loginCommand ?? (loginCommand = new ExtendedCommand(Login));

        public ICommand RegisterCommand => registerCommand ?? (registerCommand = new ExtendedCommand(NavigateToRegisterScreen));

        /// <summary>
        /// Команда при показване на екрана
        /// </summary>
        public ICommand OnAppearingCommand => onAppearingCommand ?? (onAppearingCommand = new ExtendedCommand(OnAppearing));

        #endregion

        /// <summary>
        /// Въведената парола
        /// </summary>
        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Въведената парола
        /// </summary>
        public string Username
        {
            get => username;
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Презарежда потребителите при всяко отваряне на екрана
        /// </summary>
        /// <param name="obj"></param>
        private void OnAppearing(object obj)
        {
            Username = null;
            Password = null;
        }

        /// <summary>
        /// Валидира потребителя и извършва вход в приложението
        /// </summary>
        /// <param name="obj"></param>
        private async void Login(object obj)
        {
            if (!Validate())
            {
                return;
            }

            var user = new LoginModel()
            {
                Email = Username,
                Password = Password,
            };

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "/api/authentication/login");
                request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var response = await ApiClient.Instance.HttpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var logedUser = await response.Content.ReadAsAsync<Response<LoginResponseModel>>();

                    if (logedUser.Status == StatusEnum.Success)
                    {
                        if(!logedUser.Data.Claims.Any(c => c.Equals("Permission.Orders.Manage")))
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                DependencyService.Resolve<IMessage>().LongAlert(AppResources.strEmployeesOnly);
                            });
                            return;
                        }

                        Settings.Settings.SetToken(new Token() { AccessToken = logedUser.Data.AccessToken, RefreshToken = logedUser.Data.RefreshToken });

                        var shopsRequest = new HttpRequestMessage(HttpMethod.Get, "/api/flowershop/work-places");
                        var shopsResponse = await ApiRequestHandler.Instance.SendAuthenticatedRequestAsync(shopsRequest);

                        if (shopsResponse.IsSuccessStatusCode)
                        {
                            var responseData = await shopsResponse.Content.ReadAsAsync<Response<IEnumerable<FlowerShopDTO>>>();

                            var shops = responseData.Data;

                            if (!shops.Any())
                            {
                                MainThread.BeginInvokeOnMainThread(() =>
                                {
                                    DependencyService.Resolve<IMessage>().LongAlert(AppResources.strNoWorkPlaces);
                                });
                                return;
                            }

                            Settings.Settings.SelectedShopId = shops.FirstOrDefault().Id;
                            Settings.Settings.SetToken(new Token() { AccessToken = logedUser.Data.AccessToken, RefreshToken = logedUser.Data.RefreshToken });
                            Settings.Settings.LoggedUserEmail = user.Email;
                            Settings.Settings.LoggedUserId = logedUser.Data.ID;
                            DependencyService.Resolve<ISignalRService>().StartSignalRConnection(user.Email, AppResources.strNewOrder);

                            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                        }
                        else
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                DependencyService.Resolve<IMessage>().LongAlert(AppResources.strNoConnection);
                            });
                        }
                    }
                    else
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService.Resolve<IMessage>().LongAlert(AppResources.strInvalidUsernameOrPassword);
                        });
                    }
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        DependencyService.Resolve<IMessage>().LongAlert(AppResources.strInvalidUsernameOrPassword);
                    });
                }
            }
            catch(Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Resolve<IMessage>().LongAlert(AppResources.strNoConnection);
                });
            }
        }


        private async void NavigateToRegisterScreen(object _)
        {
            Device.OpenUri(new Uri("http://bouquet.bg/register"));
        }

        private bool Validate()
        {
            if(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                DependencyService.Resolve<IMessage>().LongAlert(AppResources.strEnterUsernameAndPassword);
                return false;
            }
            return true;
        }

        

        #endregion
    }
}
