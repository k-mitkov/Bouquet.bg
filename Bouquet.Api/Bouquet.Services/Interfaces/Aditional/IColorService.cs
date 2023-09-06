using Bouquet.Services.Models.Responses;

namespace Bouquet.Services.Interfaces.Aditional
{
    public interface IColorService
    {
        /// <summary>
        /// Взима всички цветове
        /// </summary>
        /// <returns></returns>
        Task<Response> GetColorsAsync();

        ///// <summary>
        ///// Добавяне на букет
        ///// </summary>
        ///// <returns></returns>
        //Task<Response> AddBouquetsAsync(AddBouquetRequest request);
    }
}
