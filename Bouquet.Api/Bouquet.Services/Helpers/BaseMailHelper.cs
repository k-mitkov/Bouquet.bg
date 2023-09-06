using Bouquet.Services.Interfaces.Mail;
using Bouquet.Services.Models.Mail;
using Microsoft.Extensions.Options;

namespace Bouquet.Services.Helpers
{
    public abstract class BaseMailHelper : IBaseMailHelper
    {

        #region Delcarations

        private readonly IMailService _mailService;
        private readonly MailConfiguration _configuration;

        #endregion

        #region Constructor

        public BaseMailHelper(IMailService mailService, IOptions<MailConfiguration> mailConfiguration)
        {
            _mailService = mailService;
            _configuration = mailConfiguration.Value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Wrapper for the send mail method
        /// </summary>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        public async Task SendEmail(string body,string recipient,string subject)
        {
            var xd = _configuration.From;
            await _mailService.SendAsync(new MailRequest
            {
                Body = body,
                From = _configuration.From,
                Subject = subject,
                To = recipient,
            }, true);
        }

        #endregion
    }
}
