using Bouquet.Services.Models.Requests;
using Bouquet.Services.Models.Responses;
using Microsoft.AspNetCore.Http;

namespace Bouquet.Services.Interfaces.Bouquet
{
    public interface IBouquetService
    {
        /// <summary>
        /// Взима всички букети в даден град
        /// </summary>
        /// <returns></returns>
        Task<Response> GetBouquetsAsync(string cityID, string shopID);

        /// <summary>
        /// Добавяне на букет
        /// </summary>
        /// <returns></returns>
        Task<Response> AddBouquetsAsync(AddBouquetRequest request);

        /// <summary>
        /// Качва снимки за букета
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="objectID"></param>
        /// <returns></returns>
        Task<Response> UploadPictureAsync(IFormFile file, string bouquetID);
    }
}
