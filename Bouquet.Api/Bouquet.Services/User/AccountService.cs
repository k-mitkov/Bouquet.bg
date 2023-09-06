using AutoMapper;
using Bouquet.Database;
using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Interfaces.User;
using Bouquet.Services.Models.DTOs;
using Bouquet.Services.Models.Responses;
using Bouquet.Shared.Enums;
using Bouquet.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bouquet.Services.User
{
    public class AccountService : IAccountService
    {
        #region Declarations

        private readonly URLConfiguration _urlConfig;

        private readonly UserManager<BouquetUser> _userManager;
        private readonly BouquetContext _dbContext;

        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public AccountService(IOptions<URLConfiguration> urlConfig,
                              UserManager<BouquetUser> userManager,
                              BouquetContext dbContext,
                              IMapper mapper)
        {
            _urlConfig = urlConfig.Value;
            _userManager = userManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Взима информация за потребителя чрез email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response> GetUserInfoByEmailAsync(string email)
        {
            var user = await _dbContext.Users.Include(u => u.UserInfo).FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());

            if (user == null || user.UserInfo == null)
                return new Response { Status = StatusEnum.Failure, Message = "User not found" };

            var userInfoDTO = _mapper.Map<UserInfoDTO>(user.UserInfo);
            userInfoDTO.UniqueNumber = user.UniqueNumber;

            return new Response<UserInfoDTO> { Status = StatusEnum.Success, Data = userInfoDTO };
        }

        /// <summary>
        /// Проверява дали имейла е свободен
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response> CheckEmailAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());

            if (user == null)
                return new Response { Status = StatusEnum.Success };

            return new Response { Status = StatusEnum.Failure, Message = "User with this email already exists" }; ;
        }

        /// <summary>
        /// Връща url за профилната снимка на потребителя
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response> GetProfilePictureUrlAsync(string email)
        {
            var profilePictureUrl = await _dbContext.Users.Where(e => e.Email!.ToLower() == email.ToLower()).Select(u => u.ProfilePictureDataUrl).FirstOrDefaultAsync();

            return new Response<string> { Status = StatusEnum.Success, Data = _urlConfig.AddressAPI + "/" + profilePictureUrl };
        }

        /// <summary>
        /// Актуализира информацията за потребителя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userInfoDTO"></param>
        /// <returns></returns>
        public async Task<Response> UpdateUserInfoAsync(string email, UserInfoDTO userInfoDTO)
        {
            var userInfo = await _dbContext.UserInfos.Include(u => u.User).FirstOrDefaultAsync(u => u.User.Email!.ToLower() == email.ToLower());

            if (userInfo == null)
                return new Response { Status = StatusEnum.Failure, Message = "User not found" };

            userInfo.FirstName = userInfoDTO.FirstName;
            userInfo.LastName = userInfoDTO.LastName;
            userInfo.Country = userInfoDTO.Country;
            userInfo.City = userInfoDTO.City;
            userInfo.Address = userInfoDTO.Address;
            userInfo.PhoneNumber = userInfoDTO.PhoneNumber;
            userInfo.TaxId = userInfoDTO.TaxId;
            userInfo.VatId = userInfoDTO.VatId;
            userInfo.Company = userInfoDTO.Company;

            _dbContext.UserInfos.Update(userInfo);
            await _dbContext.SaveChangesAsync();

            return new Response { Status = StatusEnum.Success };
        }

        /// <summary>
        /// Променя паролата на потребителя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<Response> ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new Response { Status = StatusEnum.Failure, Message = "User not found" };

            var result = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!result)
                return new Response { Status = StatusEnum.Failure, Message = "Wrong password" };

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!changePasswordResult.Succeeded)
                return new Response { Status = StatusEnum.Failure, Message = "Internal error" };

            return new Response { Status = StatusEnum.Success };

        }

        /// <summary>
        /// Обновява снимката на потребителя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<Response> UploadProfilePictureAsync(string email, IFormFile file)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());

                if (user == null)
                    return new Response { Status = StatusEnum.Failure, Message = "User not found" };

                string directoryPath = "Files/Profile-Pictures";

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var extension = Path.GetExtension(file.FileName);

                string uniqueFileName = $"{user.Id}-{file.FileName}{extension}";
                string filePath = Path.Combine(directoryPath, uniqueFileName);

                var filesToDelete = Directory.GetFiles(directoryPath).Where(f => f.Contains(user.Id));

                foreach (var fileToDelete in filesToDelete)
                {
                    File.Delete(fileToDelete);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                user.ProfilePictureDataUrl = filePath;
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return new Response<string> { Status = StatusEnum.Success, Data = _urlConfig.AddressAPI + "/" + filePath };
            }
            catch (Exception ex)
            {
                return new Response { Status = StatusEnum.Failure, Message = "Internal error" };
            }
        }

        #endregion
    }
}
