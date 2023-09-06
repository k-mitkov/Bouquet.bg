using Bouquet.Api.Extensions;
using Bouquet.Services.Interfaces.Authentication;
using Bouquet.Services.Interfaces.Helpers;
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
    public class PaymentsController : ControllerBase
    {
        #region Declarations

        private readonly IPaymentsService _paymentsService;
        private readonly ITokenHelper _tokenHelper;
        private readonly INotyficationHelper _notyficationHelper;
        private readonly IWalletService _walletService;

        #endregion

        #region Constructors

        public PaymentsController(IPaymentsService paymentsService, ITokenHelper tokenHelper, 
            INotyficationHelper notyficationHelper, IWalletService walletService)
        {
            _paymentsService = paymentsService;
            _tokenHelper = tokenHelper;
            _notyficationHelper = notyficationHelper;
            _walletService = walletService;
        }

        #endregion

        #region API Endpoints

        [HttpPost]
        [Route("create/existing-card")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentExistingCardRequest request)
        {
            var result = await _paymentsService.CreatePaymentWithExistingCardAsync(request.OrderId, request.CardId, request.Amount);

            if (result.Status == StatusEnum.Failure)
                return  BadRequest(result);

            await _notyficationHelper.SentNotyfication(((Response<string>)result).Data);

            await _walletService.HandlePayment(request);

            return Ok(result);
        }

        [HttpPost]
        [Route("create/new-card")]
        public async Task<IActionResult> CreatePaymentWithNewCard([FromBody] CreatePaymentNewCardRequest request)
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            var result = await _paymentsService.CreatePaymentWithNewCardAsync(request.OrderId, email, request.NewCard, request.Amount);

            if (result.Status == StatusEnum.Failure)
                return BadRequest(result);

            await _notyficationHelper.SentNotyfication(((Response<string>) result).Data);

            await _walletService.HandlePayment(request);

            return Ok(result);
        }

        #endregion
    }
}
