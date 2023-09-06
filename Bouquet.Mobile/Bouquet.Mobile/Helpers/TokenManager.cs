using Bouquet.Mobile.Enums;
using Bouquet.Mobile.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bouquet.Mobile.Helpers
{
    public class TokenManager
    {
        private Token currentToken;

        public TokenManager() { 
            currentToken = Settings.Settings.GetToken();
        }

        public Token CurrentToken
        {
            get => Settings.Settings.GetToken();
            private set
            {
                Settings.Settings.SetToken(value);
            }
        }

        public async Task<bool> RefreshTokenAsync()
        {
            try
            {
                var response = await ApiRequestHandler.Instance.SendAuthenticatedPostRequestAsync("/authentication/refresh-token", CurrentToken);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsAsync<Response<JWTTokenModel>>();

                    if (responseData.Status == StatusEnum.Success)
                    {
                        Settings.Settings.SetToken(new Token() { AccessToken = responseData.Data.AccessToken, RefreshToken = responseData.Data.RefreshToken });
                        return true;
                    }
                }
                else
                {
                    Settings.Settings.LoggedUserId = "";
                }
            }
            catch
            {
                Settings.Settings.LoggedUserId = "";
            }
            
            return false;
        }
    }
}
