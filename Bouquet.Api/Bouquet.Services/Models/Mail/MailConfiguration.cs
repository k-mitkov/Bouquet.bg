namespace Bouquet.Services.Models.Mail
{
    public class MailConfiguration
    {
        public int Port { get; set; }
        public string DisplayName { get; set; } = default!;
        public string From { get; set; } = default!;
        public string Host { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string UserName { get; set; } = default!;
    }
}
