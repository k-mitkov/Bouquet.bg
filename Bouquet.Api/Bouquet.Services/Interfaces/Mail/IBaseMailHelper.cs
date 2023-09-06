using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bouquet.Services.Interfaces.Mail
{
    public interface IBaseMailHelper
    {
        Task SendEmail(string body,string recipient,string subject);
    }
}
