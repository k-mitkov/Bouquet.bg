using Bouquet.Mobile.Enums;
using Bouquet.Mobile.Extensions;
using Bouquet.Mobile.Helpers;
using Bouquet.Mobile.Intefaces;
using Bouquet.Mobile.Models;
using Bouquet.Mobile.Resources.Resx;
using Bouquet.Mobile.Resources.Themes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseMobile.Commands;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Bouquet.Mobile.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Declarations

        private SupportedLanguage selectedLanguage;
        private IEnumerable<SupportedLanguage> supportedLanguages;
        private IEnumerable<FlowerShopDTO> objects = new List<FlowerShopDTO>() { new FlowerShopDTO() { Name = "test", Id = "" } };

        /// <summary>
        /// Команда при показване на екрана
        /// </summary>
        private ICommand onAppearingCommand;

        /// <summary>
        /// Избрания обект
        /// </summary>
        private FlowerShopDTO selectedObject;

        #endregion

        #region Properties

        /// <summary>
        /// Команда при показване на екрана
        /// </summary>
        public ICommand OnAppearingCommand => onAppearingCommand ?? (onAppearingCommand = new ExtendedCommand(OnAppearing));

        /// <summary>
        /// Състояние на бутона
        /// </summary>
        public bool IsThemeButtonToggled
        {
            get
            {
                return Settings.Settings.Theme == ThemeEnum.LightTheme ? true : false;
            }
            set
            {
                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                mergedDictionaries.Clear();

                if (value)
                {
                    Settings.Settings.Theme = ThemeEnum.LightTheme;
                    mergedDictionaries.Add(new LightTheme());
                }
                else
                {
                    Settings.Settings.Theme = ThemeEnum.DarkTheme;
                    mergedDictionaries.Add(new DarkTheme());
                }
            }
        }

        /// <summary>
        /// Избран език на програмата
        /// </summary>
        public SupportedLanguage SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                if (selectedLanguage != value)
                {
                    selectedLanguage = value;

                    Settings.Settings.Lenguage = value.LanguageType;

                    var langName = value.LanguageType.GetDescription();
                    LocalizationResourceManager.Current.CurrentCulture = langName == null ? CultureInfo.CurrentCulture : new CultureInfo(langName);

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Поддържани от програмата езици
        /// </summary>
        public IEnumerable<SupportedLanguage> SupportedLanguages
        {
            get => supportedLanguages ?? (supportedLanguages = new List<SupportedLanguage>()
            {
                new SupportedLanguage(LanguageEnum.English,"Resources/UnitedKingdom.png"),
                new SupportedLanguage(LanguageEnum.Български,"Resources/Bulgaria.png")
            });
        }

        /// <summary>
        /// Избрания обект
        /// </summary>
        public FlowerShopDTO SelectedObject
        {
            get
            {
                if (selectedObject == null)
                {
                    selectedObject = Objects.FirstOrDefault(о => о.Id == Settings.Settings.SelectedShopId) ?? Objects.FirstOrDefault();
                }

                return selectedObject;
            }
            set
            {
                if (selectedObject != value)
                {
                    selectedObject = value;
                    OnPropertyChanged();

                    if(selectedObject != null)
                    {
                        Settings.Settings.SelectedShopId = selectedObject.Id;
                    }
                }
            }
        }

        /// <summary>
        /// Списък с обекти
        /// </summary>
        public IEnumerable<FlowerShopDTO> Objects
        {
            get
            {
                return objects;
            }
            set
            {
                if (objects != value)
                {
                    objects = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructor

        public SettingsViewModel()
        {
            SelectedLanguage = SupportedLanguages.FirstOrDefault(sp => sp.LanguageType == Settings.Settings.Lenguage);
        }

        #endregion

        #region Methods

        /// Презарежда потребителите при всяко отваряне на екрана
        /// </summary>
        /// <param name="obj"></param>
        private void OnAppearing()
        {
            Task.Run(async () =>
            {
                IsBusy = true;

                await Task.Delay(250);

                try
                {
                    var shopsRequest = new HttpRequestMessage(HttpMethod.Get, "/api/flowershop/work-places");
                    var shopsResponse = await ApiRequestHandler.Instance.SendAuthenticatedRequestAsync(shopsRequest);

                    if (shopsResponse.IsSuccessStatusCode)
                    {
                        var responseData = await shopsResponse.Content.ReadAsAsync<Response<IEnumerable<FlowerShopDTO>>>();


                        if (responseData.Status == StatusEnum.Success)
                        {
                            var shops = responseData.Data;

                            Objects = shops;
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
            }).Wait();
        }

        #endregion
    }
}
