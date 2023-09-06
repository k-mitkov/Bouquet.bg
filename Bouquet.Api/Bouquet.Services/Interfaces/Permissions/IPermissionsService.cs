using Bouquet.Services.Models.Requests;
using Bouquet.Services.Models.Responses;

namespace Bouquet.Services.Interfaces.Permissions
{
    public interface IPermissionsService
    {
        /// <summary>
        /// Взима правата на потребителя по неговия email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Response> GetPermissionsByEmailAsync(string email);

        /// <summary>
        /// Взима правата на зададена роля
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<Response> GetPermissionsByRoleAsync(string role);

        /// <summary>
        /// Добавя нов permission за дадена роля
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Response> AddPermissionToRoleAsync(AddPermissionRequest request);

        /// <summary>
        /// Премахва permission от дадена роля
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Response> RemovePermissionAsync(RemovePermissionRequest request);
    }
}
