﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using Bouquet.Mobile.Resources.Resx;
using WarehouseMobile.Commands;
using Xamarin.Essentials;

namespace Bouquet.Mobile.ViewModels
{
    public class AboutPageViewModel
    {
        #region Declarations

        private ICommand openSupportLinkCommand;
        private const string MicroinvestSupportEmail = "support@travelguide.net";

        #endregion

        #region Properties

        #region Commands 

        public ICommand OpenSupportLinkCommand => openSupportLinkCommand ?? (openSupportLinkCommand = new ExtendedCommand(OpenSupportLinkAsync));

        #endregion

        /// <summary>
        /// Форматирана версия на продукта
        /// </summary>
        public string ProductVersion => $"{AppResources.ResourceManager.GetString("strVersion", AppResources.Culture)} 1.0.1";

        /// <summary>
        /// Копирайт
        /// </summary>
        public string Copyright => $"Copyright © {DateTime.Now.Year}";

        #endregion

        #region Methods

        /// <summary>
        /// Отваря форма за изпращане на имейл до 
        /// съпорта на Микроинвест
        /// </summary>
        /// <param name="_"></param>
        private async void OpenSupportLinkAsync(object _)
        {
            var message = new EmailMessage()
            {
                To = new List<string>() { MicroinvestSupportEmail }
            };

            await Email.ComposeAsync(message);
        }

        #endregion
    }
}
