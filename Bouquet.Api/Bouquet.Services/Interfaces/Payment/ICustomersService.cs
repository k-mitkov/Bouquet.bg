using Bouquet.Services.Models.DTOs;
using Bouquet.Services.Models.Responses;

namespace Bouquet.Services.Interfaces.Payment
{
    public interface ICustomersService
    {
        Task<string> AddCardAsync(string email, string cardNumber, string cardholderName, string month, string year, string cvv, string cardType);

        Task<Response> GetCustomersCardsAsync(string email);

        Task<Response> DeleteCustomersCardAsync(string email, string cardId);
    }
}
