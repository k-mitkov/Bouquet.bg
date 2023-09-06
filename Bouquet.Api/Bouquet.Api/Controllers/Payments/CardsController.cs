using Bouquet.Api.Extensions;
using Bouquet.Services.Interfaces.Authentication;
using Bouquet.Services.Interfaces.Payment;
using Bouquet.Services.Models.Requests;
using Bouquet.Services.Models.Responses;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bouquet.Api.Controllers.Payments
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardsController : ControllerBase
    {
        #region Declarations

        private readonly ICustomersService _customersService;
        private readonly ITokenHelper _tokenHelper;

        #endregion

        #region Constructors

        public CardsController(ICustomersService customersService, ITokenHelper tokenHelper)
        {
            _customersService = customersService;
            _tokenHelper = tokenHelper;
        }

        #endregion

        #region API Endpoints

        [HttpGet]
        public async Task<IActionResult> GetCards()
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            var result = await _customersService.GetCustomersCardsAsync(email);

            if (result.Status == StatusEnum.Failure)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCard([FromBody] AddCardRequest request)
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            var cardID = await _customersService.AddCardAsync(email, request.CardNumber, request.CardholderName, request.Month, request.Year, request.CVV, request.CardType);

            if (string.IsNullOrEmpty(cardID))
                return BadRequest(new Response { Status = StatusEnum.Failure, Message = "Something went wrong" });

            return Ok(new Response { Status = StatusEnum.Success });
        }

        [HttpDelete]
        [Route("delete/{cardId}")]
        public async Task<IActionResult> DeleteCard(string cardId)
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            var result = await _customersService.DeleteCustomersCardAsync(email, cardId);

            if (result.Status == StatusEnum.Failure)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion
    }
}
