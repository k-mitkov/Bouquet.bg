using Bouquet.Services.Interfaces.Authentication;
using Bouquet.Services.Interfaces.Mail;
using Bouquet.Services.Models.Authentication;
using Bouquet.Services.Models.User;
using Bouquet.Shared.Enums;
using Bouquet.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bouquet.Api.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        #region Declarations

        private readonly IJWTAuthenticationService _jWTAuthenticationService;
        private readonly IUserMailService _userMailService;
        private readonly URLConfiguration _urlConfig;

        #endregion

        #region Constructor

        public AuthenticationController(IJWTAuthenticationService jWTAuthenticationService, 
            IUserMailService userMailService, 
            IOptions<URLConfiguration> urlConfig)
        {
            _jWTAuthenticationService = jWTAuthenticationService;
            _userMailService = userMailService;
            _urlConfig = urlConfig.Value;
        }

        #endregion

        #region API Endpoints

        /// <summary>
        /// Endpoint for logging in
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
			var response = await _jWTAuthenticationService.Login(loginModel);
            if (response.Status == StatusEnum.Failure)
                return Unauthorized(response);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint for registration
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInfo registerModel)
        {
            var response = await _jWTAuthenticationService.Register(registerModel);

            if (response.Status == StatusEnum.Failure)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint for refreshing jwt token
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(JWTTokenModel tokenModel)
        {
            var response = await _jWTAuthenticationService.RefreshToken(tokenModel);

            if (response.Status == StatusEnum.Failure)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Confirms newly
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery]  string code)
        {
            await _userMailService.ConfirmEmail(userId,code);

            return Redirect($"{_urlConfig.AddressWebClient}/login");
        }

		#endregion
	}
}
