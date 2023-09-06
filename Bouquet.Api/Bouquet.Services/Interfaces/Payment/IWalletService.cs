using Bouquet.Services.Models.Requests;

namespace Bouquet.Services.Interfaces.Payment
{
    public interface IWalletService
    {
        public Task HandlePayment(CreatePaymentRequestBase payment);
    }
}
