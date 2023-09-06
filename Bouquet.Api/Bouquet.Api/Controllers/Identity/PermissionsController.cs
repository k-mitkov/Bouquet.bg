using Bouquet.Api.Extensions;
using Bouquet.Services.Interfaces.Authentication;
using Bouquet.Services.Interfaces.Permissions;
using Bouquet.Services.Models.Requests;
using Bouquet.Shared.Constants;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bouquet.Api.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        #region Declarations

        private readonly ITokenHelper _tokenHelper;
        private readonly IPermissionsService _permissionsService;

        #endregion

        #region Constructors

        public PermissionsController(ITokenHelper tokenHelper, IPermissionsService permissionsService)
        {
            _tokenHelper = tokenHelper;
            _permissionsService = permissionsService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Взима правата, които има потрябителят
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            if (string.IsNullOrEmpty(email))
                return BadRequest("Invalid or missing access token");

            var result = await _permissionsService.GetPermissionsByEmailAsync(email);

            if (result.Status == StatusEnum.Failure)
                return BadRequest(result);

            return Ok(result);

        }

        /// <summary>
        /// Добаяв нов permission към роля
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = RoleClaim.AddPermission)]
        [HttpPost("add-permission")]
        public async Task<IActionResult> AddPermission([FromBody] AddPermissionRequest request)
        {
            var result = await _permissionsService.AddPermissionToRoleAsync(request);
            if (result.Status == StatusEnum.Failure)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Премахва съществуващ permission от роля
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = RoleClaim.RemovePermission)]
        [HttpPost("remove-permission")]
        public async Task<IActionResult> RemovePermission([FromBody] RemovePermissionRequest request)
        {
            var result = await _permissionsService.RemovePermissionAsync(request);
            if (result.Status == StatusEnum.Failure)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion
    }
}
