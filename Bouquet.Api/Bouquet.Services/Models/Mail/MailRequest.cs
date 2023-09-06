using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bouquet.Services.Models.Mail
{
    public class MailRequest
    {
        public string To { get; init; } = default!;

        public string Subject { get; init; } = default!;

        public string Body { get; init; } = default!;

        public string From { get; init; } = default!;
    }
}
