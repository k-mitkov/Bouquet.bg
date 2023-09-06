using Bouquet.Services.Models.Responses;

namespace Bouquet.Services.Interfaces.Aditional
{
    public interface ICityService
    {
        /// <summary>
        /// Взима всички градове
        /// </summary>
        /// <returns></returns>
        Task<Response> GetCitiesAsync();
    }
}
