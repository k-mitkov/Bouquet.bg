using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Models.Responses;
using Bouquet.Services.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bouquet.Services.Interfaces.Mail
{
    public interface IUserMailService
    {
        Task<Response> ConfirmEmail(string userId, string code);
        Task ActivateAccount(BouquetUser user);
        Task<Response> RequestResetPassword(string email);
        Task<Response> ResetPassword(ResetPasswordRequest resetPasswordRequest);
    }
}
