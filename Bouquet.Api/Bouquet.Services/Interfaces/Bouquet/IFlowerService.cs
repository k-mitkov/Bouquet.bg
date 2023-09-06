using Bouquet.Services.Models.Responses;

namespace Bouquet.Services.Interfaces.Bouquet
{
    public interface IFlowerService
    {
        /// <summary>
        /// Взима всички цветя
        /// </summary>
        /// <returns></returns>
        Task<Response> GetFlowersAsync();

        ///// <summary>
        ///// Добавяне на букет
        ///// </summary>
        ///// <returns></returns>
        //Task<Response> AddBouquetsAsync(AddBouquetRequest request);
    }
}
