using Bouquet.Services.Models.Mail;

namespace Bouquet.Services.Interfaces.Mail
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request, bool isHtml);
    }
}
