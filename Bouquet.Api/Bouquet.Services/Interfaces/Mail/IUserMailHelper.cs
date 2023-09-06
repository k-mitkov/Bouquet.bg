using Bouquet.Database.Entities.Identity;

namespace Bouquet.Services.Interfaces.Mail
{
    public interface IUserMailHelper : IBaseMailHelper
    {
        /// <summary>
        /// Изтрива непотвърден потребител
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task DeleteUnconfirmedUser(string email);

        /// <summary>
        /// Генерира линк за нулиране на паролата
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GenerateResetPasswordLink(BouquetUser user);

        /// <summary>
        /// Генерира линк за потвърждаване на email
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GenerateVerifyEmailLink(BouquetUser user);

    }
}
