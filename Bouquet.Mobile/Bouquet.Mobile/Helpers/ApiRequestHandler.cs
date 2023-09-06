using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bouquet.Mobile.Helpers
{
    public class ApiRequestHandler
    {
        private static readonly Lazy<ApiRequestHandler> lazy = new Lazy<ApiRequestHandler>(() => new ApiRequestHandler());

        public static ApiRequestHandler Instance => lazy.Value;

        private readonly TokenManager tokenManager = new TokenManager();

        public async Task<HttpResponseMessage> SendAuthenticatedPostRequestAsync(string endpoint, object data)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Content = jsonContent;

            var response = await SendAuthenticatedRequestAsync(request);

            return response;
        }

        public async Task<HttpResponseMessage> SendAuthenticatedRequestAsync(HttpRequestMessage request)
        {
            // Add authorization headers
            var token = tokenManager.CurrentToken;
            request.Headers.Add("Authorization", "Bearer " + token.AccessToken);

            // Send request
            var response = await ApiClient.Instance.HttpClient.SendAsync(request);

            // Handle token refresh if needed
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                bool refreshed = await tokenManager.RefreshTokenAsync();
                if (refreshed)
                {
                    // Retry the original request with the new token
                    request.Headers.Remove("Authorization");
                    request.Headers.Add("Authorization", "Bearer " + tokenManager.CurrentToken.AccessToken);
                    response = await ApiClient.Instance.HttpClient.SendAsync(request);
                }
            }

            return response;
        }
    }
}
