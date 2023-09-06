using Bouquet.Services.Interfaces.Aditional;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bouquet.Api.Controllers.Aditionals
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        #region Declarations

        private readonly ICityService _cityService;

        #endregion

        #region Constructors

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        #endregion

        #region API Endpoints

        #region Get Methods

        /// <summary>
        /// Взима всички градове
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            var result = await _cityService.GetCitiesAsync();

            if (result.Status == StatusEnum.Failure)
                return NotFound(result);

            return Ok(result);
        }

        #endregion

        #region Post Methods

        ///// <summary>
        ///// Добавяне на град
        ///// </summary>
        ///// <returns></returns>
        //[Authorize(Policy = RoleClaim.AddBouquet)]
        //[HttpPost]
        //public async Task<IActionResult> AddBouquet([FromBody] AddBouquetRequest bouquetRequest)
        //{
        //    var result = await _cityService.AddCityAsync(bouquetRequest);

        //    if (result.Status == StatusEnum.Failure)
        //        return BadRequest(result);

        //    return Ok(result);
        //}

        #endregion

        #endregion
    }
}
