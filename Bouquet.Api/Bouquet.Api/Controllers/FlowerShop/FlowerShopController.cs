using Bouquet.Api.Extensions;
using Bouquet.Services.Interfaces.Authentication;
using Bouquet.Services.Interfaces.FlowerShop;
using Bouquet.Services.Interfaces.User;
using Bouquet.Services.Models.Requests;
using Bouquet.Services.Models.Responses;
using Bouquet.Shared.Constants;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bouquet.Api.Controllers.FlowerShop
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FlowerShopController : ControllerBase
    {
        #region Declarations

        private readonly IFlowerShopService _flowerShopService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IUserService _userService;

        #endregion

        #region Constructors

        public FlowerShopController(IFlowerShopService flowerShopService, ITokenHelper tokenHelper, IUserService userService)
        {
            _flowerShopService = flowerShopService;
            _tokenHelper = tokenHelper;
            _userService = userService;
        }

        #endregion

        #region API Endpoints

        #region Get Methods

        /// <summary>
        /// Взима всички обекти
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetObjects([FromQuery] string? shopID)
        {
            Services.Models.Responses.Response result;

            if (string.IsNullOrWhiteSpace(shopID))
            {
                result = await _flowerShopService.GetShopsAsync();
            }
            else
            {
                result = await _flowerShopService.GetShopAsync(shopID);
            }


            if (result.Status == StatusEnum.Failure)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Взима обекти на които е собственик
        /// </summary>
        /// <returns></returns>
        [HttpGet("my-shops")]
        [Authorize(Policy = RoleClaim.AddFlowerShop)]
        public async Task<IActionResult> GetOwnedObjects()
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            var result = await _flowerShopService.GetOwnedShopsAsync(email);

            if (result.Status == StatusEnum.Failure)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Взима обекти в които потребителя има право да работи
        /// </summary>
        /// <returns></returns>
        [HttpGet("work-places")]
        [Authorize(Policy = RoleClaim.ManageOrders)]
        public async Task<IActionResult> GetWorkPlacesObjects()
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            var result = await _flowerShopService.GetWorkPlacesAsync(email);

            if (result.Status == StatusEnum.Failure)
                return NotFound(result);

            return Ok(result);
        }

        /// Взима обекти на които е собственик
        /// </summary>
        /// <returns></returns>
        [HttpGet("workers")]
        [Authorize(Policy = RoleClaim.AddWorker)]
        public async Task<IActionResult> GetWorkers([FromQuery] string shopID)
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            var result = await _flowerShopService.GetWorkersAsync(shopID, email);

            if (result.Status == StatusEnum.Failure)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Връща url за снимка на обекта
        /// </summary>
        /// <returns></returns>
        /// [AllowAnonymous]
        [HttpGet("shop-picture")]
        public async Task<IActionResult> GetObjectPictureUrl([FromQuery] string objectID)
        {
            if (string.IsNullOrEmpty(objectID))
                return Forbid();

            var result = await _flowerShopService.GetPictureUrlAsync(objectID);

            return Ok(result);
        }

        #endregion

        #region Post Methods

        /// <summary>
        /// Добавяне на обект
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = RoleClaim.AddFlowerShop)]
        [HttpPost]
        public async Task<IActionResult> AddObject([FromBody] AddFlowerShopRequest shopRequest)
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            var result = await _flowerShopService.AddShopAsync(shopRequest, email);

            if (result.Status == StatusEnum.Failure)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Добавяне на обект
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = RoleClaim.AddWorker)]
        [HttpPost("add-worker")]
        public async Task<IActionResult> AddWorker([FromBody] AddWorkerRequest addWorkerRequest)
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            var result = await _flowerShopService.AddWorkerAsync(addWorkerRequest, email);

            if (result.Status == StatusEnum.Failure)
                return BadRequest(result);

            var result2 = await _userService.AddWorkerRole(addWorkerRequest.UserUniqueNumber);

            if (result2.Status == StatusEnum.Failure)
                return BadRequest(result2);

            return Ok(result2);
        }

        /// <summary>
        /// Обновява снимката на обекта
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        [Authorize(Policy = RoleClaim.AddFlowerShop)]
        [HttpPost("upload-shop-picture")]
        public async Task<IActionResult> UploadObjectPicture([FromForm] IFormCollection formData, [FromQuery] string shopID)
        {
            if (formData == null || formData.Files.Count == 0)
                return BadRequest();

            IFormFile file = formData.Files.GetFile("file");

            if (file == null || file.Length == 0)
                return BadRequest();

            var result = await _flowerShopService.UploadPictureAsync(file, shopID);

            if (result.Status == StatusEnum.Failure)
                return StatusCode(StatusCodes.Status500InternalServerError, result);

            return Ok(result);
        }

        #endregion

        #region Delete Methods

        /// <summary>
        /// Премахване на потребител
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = RoleClaim.AddWorker)]
        [HttpDelete("remove-worker")]
        public async Task<IActionResult> RemoveWorker([FromQuery] string objectID, [FromQuery] string workerId)
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            var result = (Response<bool>) await _flowerShopService.RemoveWorkerAsync(workerId, objectID, email);

            if (result.Status == StatusEnum.Failure)
                return BadRequest(result);

            if (!result.Data)
            {
                var result2 = await _userService.RemoveWorkerRole(workerId);

                if (result2.Status == StatusEnum.Failure)
                    return BadRequest(result2);

                return Ok(result2);
            }

            return Ok(result);
        }

        #endregion

        #endregion
    }
}
