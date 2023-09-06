using Bouquet.Database;
using Bouquet.Database.Entities.Payments;
using Bouquet.Services.Interfaces.Payment;
using Bouquet.Services.Models.Requests;
using Bouquet.Services.Models.Responses;
using Bouquet.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Bouquet.Services.Payments
{
    public class PaymentsService : IPaymentsService
    {
        private readonly BouquetContext _dbContext;

        private readonly ICustomersService _customersService;

        public PaymentsService(BouquetContext dbContext,
                               ICustomersService customersService)
        {
            _dbContext = dbContext;
            _customersService = customersService;
        }

        public async Task<Response> CreatePaymentWithExistingCardAsync(string orderId, string cardId, string amount)
        {
            return await CreatePaymentAsync(orderId, cardId, amount);
        }

        public async Task<Response> CreatePaymentWithNewCardAsync(string orderId, string email, AddCardRequest newCard, string amount)
        {
            var cardId = await _customersService.AddCardAsync(email, newCard.CardNumber, newCard.CardholderName, newCard.Month, newCard.Year, newCard.CVV, newCard.CardType);

            if (string.IsNullOrEmpty(cardId))
                return new Response { Status = StatusEnum.Failure, Message = "Something went wrong" };

            return await CreatePaymentAsync(orderId, cardId, amount);
        }

        private async Task<Response> CreatePaymentAsync(string orderId, string cardId, string amount)
        {
            try
            {
                var card = _dbContext.Cards.Include(c => c.User).FirstOrDefault(c => c.Id == cardId);
                var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                if (card == null)
                    return new Response { Status = StatusEnum.Failure, Message = "Card not found" };

                decimal.TryParse(amount, out var decimalAmount);

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(decimalAmount * 100),
                    Currency = "BGN",
                    PaymentMethod = card.CardId,
                    Customer = card.User.CutomerId
                };

                var service = new PaymentIntentService();
                var result = service.Create(options);

                var payment = new Payment
                {
                    Amount = decimalAmount,
                    Card = card,
                    DateOfPayment = DateTime.Now,
                    OrderId = orderId,
                    PaymentId = result.Id
                };

                order.Status = (int) OrderStatusEnum.Paid;

                _dbContext.Update(order);
                await _dbContext.Payments.AddAsync(payment);
                await _dbContext.SaveChangesAsync();

                return new Response<string> { Status = StatusEnum.Success, Data = order.FlowerShopID};
            }
            catch (Exception ex)
            {
                return new Response { Status = StatusEnum.Failure, Message = "Something went wrong" };
            }
        }
    }
}