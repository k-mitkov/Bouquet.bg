using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Interfaces.Mail;
using Bouquet.Services.Models.Responses;
using Bouquet.Services.Models.User;
using Bouquet.Shared.Enums;
using Bouquet.Shared.Resources;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using System.Text;

namespace Bouquet.Services.Mail
{
    public class UserMailService : IUserMailService
    {

        #region Declarations

        private readonly UserManager<BouquetUser> _userManager;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IUserMailHelper _userMailHelper;
        private readonly IStringLocalizer<ApiTranslation> _localizer;

        #endregion

        #region Constructor

        public UserMailService(UserManager<BouquetUser> userManager,
                               IBackgroundJobClient backgroundJobClient,
                               IUserMailHelper userMailHelper,
                               IStringLocalizer<ApiTranslation> localizer)
        {
            _userManager = userManager;
            _backgroundJobClient = backgroundJobClient;
            _userMailHelper = userMailHelper;
            _localizer = localizer;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Wrapper method that sends an email for verification and then schedules 
        /// a delete of the user in case the email is not confirmed 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task ActivateAccount(BouquetUser user)
        {
            var verificationUri = await _userMailHelper.GenerateVerifyEmailLink(user);

            _backgroundJobClient.Enqueue(() => _userMailHelper.SendEmail(string.Format(_localizer["Confirm email template"], user.Email, verificationUri), user.Email!, _localizer["Email verifcation"]));

            _backgroundJobClient.Schedule<IUserMailHelper>(userMailHelper => userMailHelper.DeleteUnconfirmedUser(user.Email!), TimeSpan.FromMinutes(30));

        }

        /// <summary>
        /// Updates the electric stations user email to confirmed
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<Response> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                new Response() { Status = StatusEnum.Failure, Message = "User not found" };


            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
                return new Response() { Status = StatusEnum.Failure, Message = "Confirmation failed" };

            return new Response() { Status = StatusEnum.Success, Message = "User email confirmed" };
        }

        /// <summary>
        /// Requests a password reset
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response> RequestResetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new Response() { Status = StatusEnum.Failure, Message = "User not found" };

            var resetPasswordUri = await _userMailHelper.GenerateResetPasswordLink(user);

            _backgroundJobClient.Enqueue<IUserMailHelper>(userMailHelper => userMailHelper.SendEmail(string.Format(_localizer["Reset password email template"], user.Email, resetPasswordUri), user.Email, _localizer["Password reset"]));

            return new Response() { Status = StatusEnum.Success };
        }

        /// <summary>
        /// Resets the user password
        /// </summary>
        /// <param name="resetPasswordRequest"></param>
        /// <returns></returns>
        public async Task<Response> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);

            if (user == null)
                return new Response() { Status = StatusEnum.Failure, Message = "User not found" };

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordRequest.Token, resetPasswordRequest.NewPassword);

            if (!result.Succeeded)
                return new Response() { Status = StatusEnum.Failure };

            return new Response() { Status = StatusEnum.Success, Message = "Password reset successfully" };

        }

        #endregion

    }
}
