using AutoMapper;
using Bouquet.Database;
using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Interfaces.User;
using Bouquet.Services.Models.DTOs;
using Bouquet.Services.Models.Responses;
using Bouquet.Shared.Constants;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bouquet.Services.User
{
    public class UserService : IUserService
    {
        #region Declarations

        private readonly UserManager<BouquetUser> _userManager;
        private readonly BouquetContext _dbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public UserService(UserManager<BouquetUser> userManager, BouquetContext dbContext, IMapper mapper)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Връща потребителите регистрирани като компания
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response> GetCompanyAccountsAsync()
        {
            var users = await _dbContext.Users.Include(u => u.UserInfo).Where(u => !string.IsNullOrEmpty(u.UserInfo.Company)).ToListAsync();

            var usersNotPartners = new List<BouquetUser>();

            foreach (var user in users)
            {
                if (!(await _userManager.IsInRoleAsync(user, Role.Partner)))
                {
                    usersNotPartners.Add(user);
                }
            }

            if (usersNotPartners == null || usersNotPartners.Count == 0)
                return new Response { Status = StatusEnum.Failure, Message = "Users not found" };

            var usersInfoDTO = _mapper.Map<List<UserInfoDTO>>(usersNotPartners.Select(u => u.UserInfo).ToList());

            for (int i = 0; i < usersInfoDTO.Count; i++)
            {
                usersInfoDTO[i].UniqueNumber = usersNotPartners[i].UniqueNumber;
            }

            return new Response<List<UserInfoDTO>> { Status = StatusEnum.Success, Data = usersInfoDTO };
        }

        /// <summary>
        /// Генерира нов UniqueNumber, като взима последния създаден за дадената роля и добавя 1 към него
        /// </summary>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<string> GenerateUserUniqueNumber(string email, string role)
        {
            var lastUser = (await _userManager.GetUsersInRoleAsync(role)).OrderByDescending(u => u.UniqueNumber).FirstOrDefault();

            if (lastUser == null)
            {
                char roleLetter;
                switch (role)
                {
                    case Role.Admin:
                        roleLetter = 'A';
                        break;
                    case Role.Worker:
                        roleLetter = 'W';
                        break;
                    case Role.Partner:
                        roleLetter = 'P';
                        break;
                    case Role.Client:
                        roleLetter = 'U';
                        break;
                    default:
                        return "";
                }
                return roleLetter + "00000001";
            }

            var uniqueNumber = lastUser.UniqueNumber.Substring(1);
            var roleIdentification = lastUser.UniqueNumber[0];

            int.TryParse(uniqueNumber, out var castedUniqueNumber);

            castedUniqueNumber++;
            var newUniqueNumber = roleIdentification + castedUniqueNumber.ToString().PadLeft(8, '0');

            return newUniqueNumber;
        }

        /// <summary>
        ///  Добавя роля партньор на потребител
        /// </summary>
        /// <param name="userUniqueNumber"></param>
        /// <returns></returns>
        public async Task<Response> MakePartnerAsync(string userUniqueNumber)
        {
            try
            {
                var partner = await _dbContext.Users.FirstOrDefaultAsync(u => u.UniqueNumber.ToLower() == userUniqueNumber.ToLower());

                if (partner == null)
                    return new Response { Status = StatusEnum.Failure, Message = "User not found" };

                var roles = await _userManager.GetRolesAsync(partner);

                if (!roles.Contains(Role.Partner))
                {
                    if (!partner.UniqueNumber.StartsWith('P'))
                    {
                        partner.UniqueNumber = await GenerateUserUniqueNumber(partner.Email, Role.Partner);

                        _dbContext.Update(partner);
                    }

                    await _userManager.AddToRoleAsync(partner, Role.Partner);

                    await _dbContext.SaveChangesAsync();
                }

                return new Response { Status = StatusEnum.Success };
            }
            catch
            {
                return new Response { Status = StatusEnum.Failure, Message = "Something went wrong" };
            }

        }

        /// <summary>
        /// Добавя роля служител на потребител
        /// </summary>
        /// <param name="userUniqueNumber"></param>
        /// <returns></returns>
        public async Task<Response> AddWorkerRole(string userUniqueNumber)
        {
            try
            {
                var worker = await _dbContext.Users.FirstOrDefaultAsync(u => u.UniqueNumber.ToLower() == userUniqueNumber.ToLower());

                if (worker == null)
                    return new Response { Status = StatusEnum.Failure, Message = "User not found" };

                var roles = await _userManager.GetRolesAsync(worker);

                if (!roles.Contains(Role.Worker))
                {
                    if (worker.UniqueNumber.StartsWith('U'))
                    {
                        worker.UniqueNumber = await GenerateUserUniqueNumber(worker.Email, Role.Worker);

                        _dbContext.Update(worker);
                    }

                    await _userManager.AddToRoleAsync(worker, Role.Worker);

                    await _dbContext.SaveChangesAsync();
                }

                return new Response { Status = StatusEnum.Success };
            }
            catch
            {
                return new Response { Status = StatusEnum.Failure, Message = "Something went wrong" };
            }
            
        }

        /// <summary>
        /// Премахване роля служител на потребител
        /// </summary>
        /// <param name="userUniqueNumber"></param>
        /// <returns></returns>
        public async Task<Response> RemoveWorkerRole(string workerId)
        {
            try
            {
                var worker = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == workerId);

                if (worker == null)
                    return new Response { Status = StatusEnum.Failure, Message = "User not found" };

                var roles = await _userManager.GetRolesAsync(worker);

                if (!roles.Contains(Role.Worker))
                {
                    return new Response { Status = StatusEnum.Failure, Message = "User is not worker" };
                }

                worker.UniqueNumber = await GenerateUserUniqueNumber(worker.Email, Role.Client);

                _dbContext.Update(worker);

                await _userManager.RemoveFromRoleAsync(worker, Role.Worker);

                await _dbContext.SaveChangesAsync();

                return new Response { Status = StatusEnum.Success };
            }
            catch
            {
                return new Response { Status = StatusEnum.Failure, Message = "Something went wrong" };
            }
            
        }

        #endregion
    }
}
