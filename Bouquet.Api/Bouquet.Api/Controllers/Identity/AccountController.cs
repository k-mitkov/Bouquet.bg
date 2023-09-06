using Bouquet.Api.Extensions;
using Bouquet.Services.Interfaces.Authentication;
using Bouquet.Services.Interfaces.User;
using Bouquet.Services.Models.DTOs;
using Bouquet.Services.Models.Requests;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bouquet.Api.Controllers.Identity
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Declarations

        private readonly ITokenHelper _tokenHelper;

        private readonly IAccountService _accountService;

        #endregion

        #region Constructors

        public AccountController(ITokenHelper tokenHelper,
                                 IAccountService accountService)
        {
            _tokenHelper = tokenHelper;
            _accountService = accountService;
        }


        #endregion

        #region Endpoints

        #region Get Methods

        /// <summary>
        /// Връща инфорамция за потребителя
        /// </summary>
        /// <returns></returns>
        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            if (string.IsNullOrEmpty(email))
                return Forbid();

            var result = await _accountService.GetUserInfoByEmailAsync(email);

            if (result.Status == StatusEnum.Failure)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Връща url за профилната снимка на потребителя
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile-picture")]
        public async Task<IActionResult> GetProfilePictureUrl()
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            if (string.IsNullOrEmpty(email))
                return Forbid();

            var result = await _accountService.GetProfilePictureUrlAsync(email);

            return Ok(result);
        }

        /// <summary>
        /// Проверява дали имейла е вече регистриран
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        [HttpGet("check-email")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmail([FromQuery] string email)
        {
            var response = await _accountService.CheckEmailAsync(email);

            if (response.Status == StatusEnum.Failure)
                return BadRequest(response);

            return Ok(response);
        }

        #endregion

        #region Post Methods

        /// <summary>
        /// Обновява снимката на потребителя
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPost("upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] IFormCollection formData)
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            if (formData == null)
                return BadRequest();

            IFormFile file = formData.Files.GetFile("file");

            if (file == null || file.Length == 0)
                return BadRequest();

            var result = await _accountService.UploadProfilePictureAsync(email, file);

            if (result.Status == StatusEnum.Failure)
                return StatusCode(StatusCodes.Status500InternalServerError, result);

            return Ok(result);
        }


        #endregion

        #region Put Methods

        /// <summary>
        /// Актуализира информацията за потребителя
        /// </summary>
        /// <param name="userInfoDTO"></param>
        /// <returns></returns>
        [HttpPut("user-info")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfoDTO userInfoDTO)
        {
            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            if (string.IsNullOrEmpty(email))
                return Forbid();

            var result = await _accountService.UpdateUserInfoAsync(email, userInfoDTO);

            if (result.Status == StatusEnum.Failure)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Променя паролата на потребителя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.OldPassword) || string.IsNullOrEmpty(request.NewPassword))
                return BadRequest();

            var email = Request.GetEmailFromAccessToken(_tokenHelper);

            if (string.IsNullOrEmpty(email))
                return Forbid();

            var result = await _accountService.ChangePasswordAsync(email, request.OldPassword, request.NewPassword);

            if (result.Status == StatusEnum.Failure)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #endregion
    }
}
