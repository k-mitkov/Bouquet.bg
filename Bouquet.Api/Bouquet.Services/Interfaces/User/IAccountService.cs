using Bouquet.Services.Models.DTOs;
using Bouquet.Services.Models.Responses;
using Microsoft.AspNetCore.Http;

namespace Bouquet.Services.Interfaces.User
{
    public interface IAccountService
    {
        /// <summary>
        /// Взима информация за потребителя чрез email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Response> GetUserInfoByEmailAsync(string email);

        /// <summary>
        /// Проверява дали имейла е свободен
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Response> CheckEmailAsync(string email);

        /// <summary>
        /// Връща url за профилната снимка на потребителя
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Response> GetProfilePictureUrlAsync(string email);

        /// <summary>
        /// Актуализира информацията за потребителя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userInfoDTO"></param>
        /// <returns></returns>
        Task<Response> UpdateUserInfoAsync(string email, UserInfoDTO userInfoDTO);

        /// <summary>
        /// Променя паролата на потребителя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<Response> ChangePasswordAsync(string email, string oldPassword, string newPassword);

        /// <summary>
        /// Обновява снимката на потребителя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<Response> UploadProfilePictureAsync(string email, IFormFile file);
    }
}
