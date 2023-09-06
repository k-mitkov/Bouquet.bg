using Bouquet.Services.Interfaces.Mail;
using Bouquet.Services.Interfaces.User;
using Bouquet.Services.Models.User;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bouquet.Api.Controllers.Identity
{
    [Authorize]
    [Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		#region Declarations

		private readonly IUserMailService _userMailService;
		private readonly IUserService _userService;

		#endregion

		#region Constructor

		public UserController(IUserMailService userMailService, IUserService userService)
		{
			_userMailService = userMailService;
			_userService = userService;
		}

        #endregion

        #region API Endpoints

        #region Get Methods

        // <summary>
        /// Връща потребителите регистрирани като компания
        /// </summary>
        /// <returns></returns>
        [HttpGet("users-companies")]
        [AllowAnonymous]
        //[Authorize(Policy = RoleClaim.ManageUsers)]
        public async Task<IActionResult> GetCompanyAccounts()
        {
            var result = await _userService.GetCompanyAccountsAsync();

            if (result.Status == StatusEnum.Failure)
                return NotFound(result);

            return Ok(result);
        }


        #endregion

        #region Post Methods

        /// <summary>
        /// Сменя ролята на партньор
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("make-partner")]
        //[Authorize(Policy = RoleClaim.ManageUsers)]
        public async Task<IActionResult> MakePartner([FromBody] string uniqueNumber)
        {
            var response = await _userService.MakePartnerAsync(uniqueNumber);
            return Ok(response);
        }

        /// <summary>
        /// Api Endpoint for requsting reset password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AllowAnonymous]
		[HttpPost("forgot-password")]
		public async Task<IActionResult> RequestResetPassword([FromQuery] string email)
		{
			var response = await _userMailService.RequestResetPassword(email);
			return Ok(response);
		}

		/// <summary>
		/// Confirms newly
		/// </summary>
		/// <param name="resetPasswordRequest"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
		{
			var response = await _userMailService.ResetPassword(resetPasswordRequest);
			return Ok(response);
		}

        #endregion

        #endregion
    }
}
