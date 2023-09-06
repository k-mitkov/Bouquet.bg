using Bouquet.Services.Interfaces.Mail;
using Bouquet.Services.Models.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Bouquet.Services.Mail
{
    public class MailService : IMailService
    {
        #region Declarations

        private readonly MailConfiguration _config;
        private readonly ILogger<MailService> _logger;

        #endregion

        #region Constructor

        public MailService(IOptions<MailConfiguration> config, ILogger<MailService> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends Mail
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isHtml"></param>
        /// <returns></returns>
        public async Task SendAsync(MailRequest request, bool isHtml)
        {
            using var smtp = new SmtpClient
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = _config.Host,
                Port = _config.Port,
                Credentials = new NetworkCredential { UserName = _config.UserName, Password = _config.Password, },
            };
            var fromEmail = new MailAddress(request.From, _config.DisplayName);
            var toEmail = new MailAddress(request.To, request.To);

            var message = new MailMessage
            {
                From = fromEmail,
                Subject = request.Subject,
                Body = request.Body,
                IsBodyHtml = isHtml,
            };

            message.To.Add(toEmail);

            try
            {
                await smtp.SendMailAsync(message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
            }
        }

        #endregion
    }
}
