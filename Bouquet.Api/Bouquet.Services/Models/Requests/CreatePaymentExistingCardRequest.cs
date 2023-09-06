namespace Bouquet.Services.Models.Requests
{
    public class CreatePaymentExistingCardRequest : CreatePaymentRequestBase
    {
        public string CardId { get; set; } = string.Empty;
    }
}
