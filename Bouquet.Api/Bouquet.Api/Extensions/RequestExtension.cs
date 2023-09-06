using Bouquet.Services.Interfaces.Authentication;
using System.Net.Http.Headers;

namespace Bouquet.Api.Extensions
{
    public static class RequestExtension
    {
        /// <summary>
        /// Взима Email-ът на потребителя от токенът му за аутентикация
        /// </summary>
        /// <param name="request"></param>
        /// <param name="tokenHelper"></param>
        /// <returns></returns>
        public static string GetEmailFromAccessToken(this HttpRequest request, ITokenHelper tokenHelper)
        {
            string accessToken;

            var refreshTokenHeader = request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(refreshTokenHeader))
                return string.Empty;

            AuthenticationHeaderValue.TryParse(refreshTokenHeader, out var headerValue);

            if (headerValue == null || headerValue.Parameter == null)
                return string.Empty;

            accessToken = headerValue.Parameter;

            if (string.IsNullOrEmpty(accessToken))
                return string.Empty;

            var principal = tokenHelper.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null || principal.Identity == null || principal.Identity.Name == null)
            {
                return string.Empty;
            }

            string email = principal.Identity.Name;

            return email;
        }
    }
}
