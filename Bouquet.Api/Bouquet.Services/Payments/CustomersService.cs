using AutoMapper;
using Bouquet.Database;
using Bouquet.Services.Interfaces.Payment;
using Bouquet.Services.Models.DTOs;
using Bouquet.Services.Models.Responses;
using Bouquet.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Bouquet.Services.Payments
{
    public class CustomersService : ICustomersService
    {
        private readonly BouquetContext _dbContext;

        private readonly IMapper _mapper;

        public CustomersService(BouquetContext dbContext,
                                IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<string> AddCardAsync(string email, string cardNumber, string cardholderName, string month, string year, string cvv, string cardType)
        {
            try
            {
                //var options = new TokenCreateOptions
                //{
                //    Card = new TokenCardOptions
                //    {
                //        Number = cardNumber,
                //        Name= cardholderName,
                //        ExpMonth= month,
                //        ExpYear = year,
                //        Cvc= cvv
                //    }
                //};

                //var tokenService = new TokenService();

                //var cardTokenResponse = await tokenService.CreateAsync(options);

                var customer = _dbContext.Users.FirstOrDefault(c => c.Email.ToLower() == email.ToLower());

                if (customer == null)
                {
                    return "";
                }

                if(customer.CutomerId == null)
                {
                    var customerOptions = new CustomerCreateOptions
                    {
                        Email = customer.Email
                    };

                    var customerService = new CustomerService();
                    var customerResponse = await customerService.CreateAsync(customerOptions);

                    customer.CutomerId = customerResponse.Id;
                    _dbContext.Users.Update(customer);
                    await _dbContext.SaveChangesAsync();
                }

                var cardOptions = new CardCreateOptions
                {
                    Source = "tok_visa"
                };

                var cardService = new CardService();

                var cardResponse = await cardService.CreateAsync(customer.CutomerId, cardOptions);

                var entity = _dbContext.Cards.Add(new Database.Entities.Payments.Card
                {
                    CardId = cardResponse.Id,
                    UserID = customer.Id,
                    Last4 = cardNumber[^4..],
                    Type = cardType
                });
                await _dbContext.SaveChangesAsync();

                return entity.Entity.Id;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public async Task<Response> GetCustomersCardsAsync(string email)
        {
            var cards = await _dbContext.Cards.Include(c => c.User).Where(c => c.User.Email.ToLower() == email.ToLower()).ToListAsync();

            return new Response<List<CardDTO>> { Status = StatusEnum.Success, Data = _mapper.Map<List<CardDTO>>(cards) };
        }

        public async Task<Response> DeleteCustomersCardAsync(string email, string cardId)
        {
            var card = await _dbContext.Cards.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == cardId);

            if (card == null || card.User.Email.ToLower() != email.ToLower())
                return new Response { Status = StatusEnum.Failure, Message = "Invalid input" };

            try
            {
                var cardService = new CardService();
                await cardService.DeleteAsync(card.User.CutomerId, card.CardId);

                _dbContext.Remove(card);
                await _dbContext.SaveChangesAsync();
                return new Response { Status = StatusEnum.Success };
            }
            catch (Exception ex)
            {
                return new Response { Status = StatusEnum.Failure, Message = "Something went wrong" };
            }
        }
    }
}