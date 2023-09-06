using Bouquet.Api.Extensions;
using Bouquet.Services.Interfaces.Authentication;
using Bouquet.Services.Interfaces.Helpers;
using Bouquet.Services.Interfaces.Order;
using Bouquet.Services.Models.Requests;
using Bouquet.Shared.Constants;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bouquet.Api.Controllers.Odrer
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        #region Declarations

        private readonly IOrderService _оrderService;
        private readonly ITokenHelper _tokenHelper;
        private readonly INotyficationHelper _notyficationHelper;
        

        #endregion

        #region Constructors

        public OrderController(IOrderService оrderService, ITokenHelper tokenHelper, INotyficationHelper notyficationHelper)
        {
            _оrderService = оrderService;
            _tokenHelper = tokenHelper;
            _notyficationHelper = notyficationHelper;
        }

        #endregion

        #region API Endpoints

        #region Get Methods

        /// <summary>
        /// Взима поръчки
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] string? shopID, [FromQuery] string? userID)
        {
            var result = await _оrderService.GetOrdersAsync(shopID, userID);

            if (result.Status == StatusEnum.Failure)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Променя статуса
        /// </summary>
        /// <returns></returns>
        [HttpGet("update-status")]
        [Authorize(Policy = RoleClaim.ManageOrders)]
        public async Task<IActionResult> UpdateStatus([FromQuery] string? orderID, [FromQuery] int? status)
        {
            var result = await _оrderService.UpdateStatus(orderID, status);

            if (result.Status == StatusEnum.Failure)
                return NotFound(result);

            return Ok(result);
        }

        #endregion

        #region Post Methods

        /// <summary>
        /// Добавяне на обект
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> MakeOder([FromBody] MakeOrderRequest orderRequest)
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            Services.Models.Responses.Response result = null;

            if (string.IsNullOrEmpty(email))
            {
                //result = await _оrderService.MakeAnonymousOrder(orderRequest);
                return Unauthorized();
            }
            else
            {
                result = await _оrderService.MakeOrder(orderRequest, email);
            }

            if (result.Status == StatusEnum.Failure)
                return BadRequest(result);

            if (!orderRequest.HasDelivery)
            {
                await _notyficationHelper.SentNotyfication(orderRequest.ShopId);
            }

            return Ok(result);
        }

        #endregion

        #endregion

        #region Private Methods

        #endregion
    }
}
