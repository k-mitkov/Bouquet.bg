
using AutoMapper;
using Bouquet.Database;
using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Interfaces.Authentication;
using Bouquet.Services.Interfaces.Mail;
using Bouquet.Services.Interfaces.User;
using Bouquet.Services.Models.Authentication;
using Bouquet.Services.Models.Responses;
using Bouquet.Services.Models.User;
using Bouquet.Shared.Enums;
using Bouquet.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bouquet.Services.Authentication
{
    public class JWTAuthenticationService : IJWTAuthenticationService
    {

        #region Declarations

        private readonly URLConfiguration _urlConfig;

        private readonly BouquetContext _dbContext;
        private readonly UserManager<BouquetUser> _userManager;

        private readonly IConfiguration _configuration;
        private readonly ITokenHelper _tokenHelper;

        private readonly IUserMailService _userMailService;
        private readonly IUserService _userService;

        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public JWTAuthenticationService(IOptions<URLConfiguration> urlConfig,
                                        BouquetContext dbContext,
                                        UserManager<BouquetUser> userManager,
                                        IConfiguration configuration,
                                        ITokenHelper tokenHelper,
                                        IUserMailService userMailService,
                                        IUserService userService,
                                        IMapper mapper)
        {
            _urlConfig = urlConfig.Value;
            _dbContext = dbContext;
            _userManager = userManager;
            _configuration = configuration;
            _tokenHelper = tokenHelper;
            _userMailService = userMailService;
            _userService = userService;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public async Task<Response> Login(LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
            {
                return new Response
                {
                    Status = StatusEnum.Failure,
                    Message = "Invalid input"
                };
            }
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                return new Response
                {
                    Status = StatusEnum.Failure,
                    Message = "User is not found"
                };
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenHelper.CreateToken(authClaims);
            var refreshToken = _tokenHelper.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            await _userManager.UpdateAsync(user);

            var claims = _dbContext.RoleClaims.Where(rc => rc.Role != null && rc.Role.Name != null
                                               && rc.Role.Name.ToLower() == userRoles[0].ToLower())
                                              .Select(r => r.ClaimValue!).ToList();

            var profilePictureUrl = "";

            if (!string.IsNullOrEmpty(user.ProfilePictureDataUrl))
            {
                profilePictureUrl = _urlConfig.AddressAPI + "/" + user.ProfilePictureDataUrl;
            }

            return new Response<LoginResponseModel>
            {
                Status = StatusEnum.Success,
                Data = new LoginResponseModel
                {
                    ID = user.Id,
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    ProfilePictureUrl = profilePictureUrl,
                    Claims = claims
                }
            };
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        public async Task<Response> Register(RegisterInfo registerModel)
        {
            if (string.IsNullOrEmpty(registerModel.UserCredentials.Email) || string.IsNullOrEmpty(registerModel.UserCredentials.Password))
            {
                return new Response { Status = StatusEnum.Failure, Message = "Invalid input" };
            }

            var userExists = await _userManager.FindByEmailAsync(registerModel.UserCredentials.Email);

            if (userExists != null)
                return new Response { Status = StatusEnum.Failure, Message = "User with this email already exists" };

            var uniqueNumber = await _userService.GenerateUserUniqueNumber(registerModel.UserCredentials.Email, "Client");

            var userInfo = _mapper.Map<UserInfo>(registerModel.UserInfo);

            var user = new BouquetUser
            {
                Email = registerModel.UserCredentials.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerModel.UserCredentials.Email,
                UniqueNumber = uniqueNumber,
                UserInfo = userInfo
                ///The rest of the properties to be added..
            };

            var result = await _userManager.CreateAsync(user, registerModel.UserCredentials.Password);

            await _userManager.AddToRoleAsync(user, "Client");

            await _userMailService.ActivateAccount(user);

            if (!result.Succeeded)
                return new Response { Status = StatusEnum.Failure, Message = "User creation failed" };

            return new Response { Status = StatusEnum.Success, Message = "User created successfully" };
        }

        /// <summary>
        /// Refreshes a jwt token
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public async Task<Response> RefreshToken(JWTTokenModel tokenModel)
        {
            if (tokenModel == null)
            {
                return new Response { Status = StatusEnum.Failure, Message = "Token is null" };
            }

            var accessToken = tokenModel.AccessToken;
            var refreshToken = tokenModel.RefreshToken;

            var principal = _tokenHelper.GetPrincipalFromExpiredToken(accessToken);

            if (principal == null)
                return new Response { Status = StatusEnum.Failure, Message = "Invalid access token or refresh token" };

            var username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return new Response { Status = StatusEnum.Failure, Message = "Invalid access token or refresh token" };

            var newAccessToken = _tokenHelper.CreateToken(principal.Claims.ToList());
            var newRefreshToken = _tokenHelper.GenerateRefreshToken();
            user.RefreshToken=newRefreshToken;

            await _userManager.UpdateAsync(user);

            return new Response<JWTTokenModel>
            {
                Status = StatusEnum.Success,
                Data = new JWTTokenModel { AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken), RefreshToken = newRefreshToken }
            };
        }

        #endregion
    }
}
