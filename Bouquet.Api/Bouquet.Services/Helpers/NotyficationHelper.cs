using Bouquet.Services.Interfaces.FlowerShop;
using Bouquet.Services.Interfaces.Helpers;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Bouquet.Services.Helpers
{
    public class NotyficationHelper : INotyficationHelper
    {
        private readonly IFlowerShopService _flowerShopService;

        public NotyficationHelper(IFlowerShopService flowerShopService)
        {
            _flowerShopService = flowerShopService;
        }

        public async Task SentNotyfication(string shopId)
        {
            try
            {
                var emails = await _flowerShopService.GetWorkersEmails(shopId);

                var jsonContent = new StringContent(JsonConvert.SerializeObject(emails), Encoding.UTF8, "application/json");

                HttpClient client;
                MediaTypeWithQualityHeaderValue mediaTypeJson;
                HttpClientHandler clientHandler;

                clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                mediaTypeJson = new MediaTypeWithQualityHeaderValue("application/json");

                client = new HttpClient(clientHandler);

                client.BaseAddress = new Uri(@"https://192.168.137.1:5001/api/messages/send");
                client.DefaultRequestHeaders.Accept.Add(mediaTypeJson);

                var response3 = await client.PostAsync(client.BaseAddress, jsonContent);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
