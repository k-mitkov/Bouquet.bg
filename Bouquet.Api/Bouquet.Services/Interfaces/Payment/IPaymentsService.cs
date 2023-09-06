using Bouquet.Services.Models.Requests;
using Bouquet.Services.Models.Responses;

namespace Bouquet.Services.Interfaces.Payment
{
    public interface IPaymentsService
    {
        Task<Response> CreatePaymentWithExistingCardAsync(string orderId, string cardId, string amount);

        Task<Response> CreatePaymentWithNewCardAsync(string orderId, string email, AddCardRequest newCard, string amount);
    }
}
