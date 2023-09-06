using Bouquet.Services.Interfaces.Bouquet;
using Bouquet.Services.Models.Requests;
using Bouquet.Shared.Constants;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bouquet.Api.Controllers.Bouquet
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BouquetController : ControllerBase
    {
        #region Declarations

        private readonly IBouquetService _bouquetService;

        #endregion

        #region Constructors

        public BouquetController(IBouquetService bouquetService)
        {
            _bouquetService = bouquetService;
        }

        #endregion

        #region API Endpoints

        #region Get Methods

        /// <summary>
        /// Взима всички букети
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBouquets([FromQuery] string? cityID,[FromQuery] string? shopID)
        {
            var result = await _bouquetService.GetBouquetsAsync(cityID, shopID);

            if (result.Status == StatusEnum.Failure)
                return NotFound(result);

            return Ok(result);
        }

        #endregion

        #region Post Methods

        /// <summary>
        /// Добавяне на букет
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = RoleClaim.AddBouquet)]
        [HttpPost]
        public async Task<IActionResult> AddBouquet([FromBody] AddBouquetRequest bouquetRequest)
        {
            var result = await _bouquetService.AddBouquetsAsync(bouquetRequest);

            if (result.Status == StatusEnum.Failure)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Обновява снимките на букета
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        [Authorize(Policy = RoleClaim.AddBouquet)]
        [HttpPost("upload-bouquet-picture")]
        public async Task<IActionResult> UploadBouquetPictures([FromForm] IFormCollection formData, [FromQuery] string bouquetID)
        {
            if (formData == null || formData.Files.Count == 0)
                return BadRequest();

            Services.Models.Responses.Response res = null;

            foreach (var file in formData.Files)
            {
                if (file == null || file.Length == 0)
                    return BadRequest();

                res = await _bouquetService.UploadPictureAsync(file, bouquetID);

                if (res.Status == StatusEnum.Failure)
                    return StatusCode(StatusCodes.Status500InternalServerError, res);
            }

            return Ok(res);
        }

        #endregion

        #endregion
    }
}
