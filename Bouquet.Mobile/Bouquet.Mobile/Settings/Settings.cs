using Bouquet.Mobile.Enums;
using Bouquet.Mobile.Models;
using Xamarin.Essentials;

namespace Bouquet.Mobile.Settings
{
    public static class Settings
    {
        private static Location location;

        public static string LogKey
        {
            get
            {
                return Preferences.Get(nameof(LogKey), null);
            }
            set
            {
                Preferences.Set(nameof(LogKey) , value);
            }
        }

        public static ThemeEnum Theme
        {
            get
            {
                return (ThemeEnum) Preferences.Get(nameof(Theme), (int)ThemeEnum.LightTheme);
            }
            set
            {
                Preferences.Set(nameof(Theme), (int)value);
            }
        }

        public static LanguageEnum Lenguage
        {
            get
            {
                return (LanguageEnum)Preferences.Get(nameof(Lenguage), (int)LanguageEnum.English);
            }
            set
            {
                Preferences.Set(nameof(Lenguage), (int)value);
            }
        }

        public static string LoggedUserId
        {
            get
            {
                return Preferences.Get(nameof(LoggedUserId), "");
            }
            set
            {
                Preferences.Set(nameof(LoggedUserId), value);
            }
        }

        public static string LoggedUserEmail
        {
            get
            {
                return Preferences.Get(nameof(LoggedUserEmail), "");
            }
            set
            {
                Preferences.Set(nameof(LoggedUserEmail), value);
            }
        }

        #region Tokens

        public static string Bearer
        {
            get
            {
                return Preferences.Get(nameof(Bearer), "");
            }
            set
            {
                Preferences.Set(nameof(Bearer), value);
            }
        }

        public static string RefreshToken
        {
            get
            {
                return Preferences.Get(nameof(RefreshToken), "");
            }
            set
            {
                Preferences.Set(nameof(RefreshToken), value);
            }
        }

        #endregion

        public static Location Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        public static string SelectedShopId
        {
            get
            {
                return Preferences.Get(nameof(SelectedShopId), "");
            }
            set
            {
                Preferences.Set(nameof(SelectedShopId), value);
            }
        }

        public static void SetToken(Token token)
        {
            Bearer = token.AccessToken;
            RefreshToken = token.RefreshToken;
        }

        public static Token GetToken()
        {
            return new Token()
            {
                AccessToken = Bearer,
                RefreshToken = RefreshToken,
            };
        }
    }
}
