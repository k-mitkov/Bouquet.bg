using Bouquet.Services.Models.Requests;
using Bouquet.Services.Models.Responses;
using Microsoft.AspNetCore.Http;

namespace Bouquet.Services.Interfaces.FlowerShop
{
    public interface IFlowerShopService
    {
        /// <summary>
        /// Взима всички обекти
        /// </summary>
        /// <returns></returns>
        Task<Response> GetShopsAsync();

        // <summary>
        /// Взима всички обекти
        /// </summary>
        /// <returns></returns>
        Task<Response> GetShopAsync(string shopId);

        // <summary>
        /// Взима обекти в които потребителя има право да работи
        /// </summary>
        /// <returns></returns>
        Task<Response> GetWorkPlacesAsync(string shopId);

        /// <summary>
        /// Взима обеките собственост на партньора
        /// </summary>
        /// <returns></returns>
        Task<Response> GetOwnedShopsAsync(string email);

        /// <summary>
        /// Взима служителите в обект
        /// </summary>
        /// <returns></returns>
        Task<Response> GetWorkersAsync(string shopID,string email);

        /// <summary>
        /// Връща url за снимка на обекта
        /// </summary>
        /// <param name="shopID"></param>
        /// <returns></returns>
        Task<Response> GetPictureUrlAsync(string shopID);

        /// <summary>
        /// Мейли отговарящи на даден обект
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        Task<List<string>> GetWorkersEmails(string shopId);

        /// <summary>
        /// Добавяне на обект
        /// </summary>
        /// <returns></returns>
        Task<Response> AddShopAsync(AddFlowerShopRequest objectRequest, string email);

        /// <summary>
        /// Добавяне на служител
        /// </summary>
        /// <returns></returns>
        Task<Response> AddWorkerAsync(AddWorkerRequest addWorkerRequest, string email);

        /// <summary>
        /// Качва снимка за обекта
        /// </summary>
        /// <param name="file"></param>
        /// <param name="shopID"></param>
        /// <returns></returns>
        Task<Response> UploadPictureAsync(IFormFile file, string shopID);

        /// <summary>
        /// Премахва служител
        /// </summary>
        /// <param name="file"></param>
        /// <param name="shopID"></param>
        /// <returns></returns>
        Task<Response> RemoveWorkerAsync(string workerId, string objectID, string email);
    }
}
