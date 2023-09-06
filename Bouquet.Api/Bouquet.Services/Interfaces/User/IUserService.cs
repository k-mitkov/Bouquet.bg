using Bouquet.Services.Models.Responses;

namespace Bouquet.Services.Interfaces.User
{
    public interface IUserService
    {
        /// <summary>
        /// Връща потребителите регистрирани като компания
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Response> GetCompanyAccountsAsync();

        /// <summary>
        /// Генерира нов UniqueNumber, като взима последния създаден за дадената роля и добавя 1 към него
        /// </summary>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<string> GenerateUserUniqueNumber(string email, string role);

        /// <summary>
        /// Добавя роля партньор на потребител
        /// </summary>
        /// <param name="userUniqueNumber"></param>
        /// <returns></returns>
        Task<Response> MakePartnerAsync(string userUniqueNumber);

        /// <summary>
        /// Добавя роля служител на потребител
        /// </summary>
        /// <param name="userUniqueNumber"></param>
        /// <returns></returns>
        Task<Response> AddWorkerRole(string userUniqueNumber);

        /// <summary>
        /// Премахване роля служител на потребител
        /// </summary>
        /// <param name="userUniqueNumber"></param>
        /// <returns></returns>
        Task<Response> RemoveWorkerRole(string workerId);

    }
}
