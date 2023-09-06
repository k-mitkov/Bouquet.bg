using Bouquet.Database;
using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Interfaces.Mail;
using Bouquet.Services.Models.Mail;
using Bouquet.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text;

namespace Bouquet.Services.Helpers
{
    public class UserMailHelper : BaseMailHelper, IUserMailHelper
    {

        #region Declarations

        private readonly URLConfiguration _urlConfig;
        private readonly UserManager<BouquetUser> _userManager;
        private readonly BouquetContext _dbContext;

        #endregion

        #region Constructor

        public UserMailHelper(IMailService mailService, 
                              IOptions<MailConfiguration> mailConfiguration,
                              IOptions<URLConfiguration> urlConfig, 
                              UserManager<BouquetUser> userManager,
                              BouquetContext dbContext) : base(mailService, mailConfiguration)
        {
            _urlConfig = urlConfig.Value;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        #endregion

        #region Methods


        /// <summary>
        /// Deletes an unconfirmed user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task DeleteUnconfirmedUser(string email)
        {
            var user = await _dbContext.Users.Include(u => u.UserInfo).FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());
            if (user != null && user.EmailConfirmed != true)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Generates a reset password link with a reset password token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="adress"></param>
        /// <returns></returns>
        public async Task<string> GenerateResetPasswordLink(BouquetUser user)
        {
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordResetToken));
            var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Email!));

            var endpointUri = new Uri($"{_urlConfig.AddressWebClient}/reset-password/");
            var resetPasswordUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "email", encodedEmail);

            var res = QueryHelpers.AddQueryString(resetPasswordUri, "token", encodedToken);
            return res;
        }


        /// <summary>
        /// Generates a link to verify a user email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public async Task<string> GenerateVerifyEmailLink(BouquetUser user)
        {

            var xd = _urlConfig.AddressAPI;
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationToken));

            var endpointUri = new Uri($"{_urlConfig.AddressAPI}/api/Authentication/confirm-email/");
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);

            return QueryHelpers.AddQueryString(verificationUri, "code", code);
        }

        #endregion

    }
}