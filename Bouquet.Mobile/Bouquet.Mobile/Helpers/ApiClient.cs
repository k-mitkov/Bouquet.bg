using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bouquet.Mobile.Helpers
{
    public class ApiClient
    {
        private static readonly Lazy<ApiClient> lazy = new Lazy<ApiClient>(() => new ApiClient());

        public static ApiClient Instance => lazy.Value;

        private HttpClient httpClient;

        private ApiClient()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(AppConstands.Url) ;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public HttpClient HttpClient => httpClient;
    }
}
