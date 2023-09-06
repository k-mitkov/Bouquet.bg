using Bouquet.Services.Interfaces.Bouquet;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bouquet.Api.Controllers.Bouquet
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FlowerController : ControllerBase
    {
        #region Declarations

        private readonly IFlowerService _flowerService;

        #endregion

        #region Constructors

        public FlowerController(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        #endregion

        #region API Endpoints

        #region Get Methods

        /// <summary>
        /// Взима всички цветя
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetFlowers()
        {
            var result = await _flowerService.GetFlowersAsync();

            if (result.Status == StatusEnum.Failure)
                return NotFound(result);

            return Ok(result);
        }

        #endregion

        #region Post Methods

        ///// <summary>
        ///// Добавяне на букет
        ///// </summary>
        ///// <returns></returns>
        //[Authorize(Policy = RoleClaim.AddBouquet)]
        //[HttpPost]
        //public async Task<IActionResult> AddBouquet([FromBody] AddBouquetRequest bouquetRequest)
        //{
        //    var result = await _bouquetService.AddBouquetsAsync(bouquetRequest);

        //    if (result.Status == StatusEnum.Failure)
        //        return BadRequest(result);

        //    return Ok(result);
        //}

        #endregion

        #endregion
    }
}
