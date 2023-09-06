using Bouquet.Database.Entities;
using Bouquet.Services.Models.Requests;
using Bouquet.Services.Models.Responses;

namespace Bouquet.Services.Interfaces.Order
{
    public interface IOrderService
    {
        /// <summary>
        /// Взима поръчки
        /// </summary>
        /// <returns></returns>
        Task<Response> GetOrdersAsync(string shopID, string userId);

        /// <summary>
        /// Променя статуса
        /// </summary>
        /// <returns></returns>
        Task<Response> UpdateStatus(string orderID, int? status);

        /// <summary>
        /// Нова поръчка
        /// </summary>
        /// <returns></returns>
        Task<Response> MakeOrder(MakeOrderRequest orderRequest, string email);

        /// <summary>
        /// Нова анонимна поръчка
        /// </summary>
        /// <returns></returns>
        Task<Response> MakeAnonymousOrder(MakeOrderRequest orderRequest);
    }
}
