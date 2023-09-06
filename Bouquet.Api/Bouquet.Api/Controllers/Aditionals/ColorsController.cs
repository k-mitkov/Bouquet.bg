using Bouquet.Services.Interfaces.Aditional;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bouquet.Api.Controllers.Aditionals
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        #region Declarations

        private readonly IColorService _colorService;

        #endregion

        #region Constructors

        public ColorsController(IColorService colorService)
        {
            _colorService = colorService;
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
            var result = await _colorService.GetColorsAsync();

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
