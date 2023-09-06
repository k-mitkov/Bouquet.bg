using Bouquet.Database;
using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Interfaces.Permissions;
using Bouquet.Services.Models.Requests;
using Bouquet.Services.Models.Responses;
using Bouquet.Shared.Constants;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bouquet.Services.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        private readonly UserManager<BouquetUser> _userManager;
        private readonly BouquetContext _dbContext;

        public PermissionsService(UserManager<BouquetUser> userManager, BouquetContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Взима правата на потребителя по неговия email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response> GetPermissionsByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new Response { Status = StatusEnum.Failure };
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles == null)
            {
                return new Response { Status = StatusEnum.Failure };
            }

            roles.ToList().ForEach(role => role = role.ToLower());

            var claims = _dbContext.RoleClaims.Where(rc => rc.Role != null && rc.Role.Name != null
                                               && roles.Contains(rc.Role.Name.ToLower()))
                                              .Select(r => r.ClaimValue).ToList();

            return new Response<IList<string?>> { Status = StatusEnum.Success, Data = claims };
        }

        /// <summary>
        /// Взима правата на зададена роля
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<Response> GetPermissionsByRoleAsync(string role)
        {
            var claims = await _dbContext.RoleClaims.Where(rc => rc.Role != null
                                                     && rc.Role.Name!.ToLower() == role.ToLower())
                                                    .Select(r => r.ClaimValue).ToListAsync();

            return new Response<List<string?>> { Status = StatusEnum.Success, Data = claims };
        }

        /// <summary>
        /// Добавя нов permission за дадена роля
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response> AddPermissionToRoleAsync(AddPermissionRequest request)
        {
            var claims = await _dbContext.RoleClaims.Where(rc => rc.Role != null && rc.Role.Name!.ToLower() == request.Role.ToLower()
                                                     && rc.ClaimValue!.ToLower() == request.Permission.ToLower())
                                                    .Select(r => r.ClaimValue).ToListAsync();

            if (claims != null && claims.Count != 0)
                return new Response { Status = StatusEnum.Failure, Message = "Permission already exists for this user" };

            var roleId = await _dbContext.Roles.Where(r => r.Name!.ToLower() == request.Role.ToLower()).Select(r => r.Id).FirstOrDefaultAsync();

            if (roleId == null)
                return new Response { Status = StatusEnum.Failure, Message = "Role not found" };

            await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
            {
                RoleId = roleId,
                ClaimType = RoleClaimType.Permission,
                ClaimValue = request.Permission,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "Admin",
                Description = request.Description,
                Group = request.Group
            });

            await _dbContext.SaveChangesAsync();

            return new Response { Status = StatusEnum.Success };
        }

        /// <summary>
        /// Премахва permission от дадена роля
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response> RemovePermissionAsync(RemovePermissionRequest request)
        {
            var claim = await _dbContext.RoleClaims.FirstOrDefaultAsync(rc => rc.Role != null
                                                                        && rc.Role.Name!.ToLower() == request.Role.ToLower()
                                                                        && rc.ClaimValue!.ToLower() == request.Permission.ToLower());

            if (claim == null)
                return new Response { Status = StatusEnum.Failure, Message = "Permission not found" };

            _dbContext.RoleClaims.Remove(claim);
            await _dbContext.SaveChangesAsync();

            return new Response { Status = StatusEnum.Success };
        }
    }
}
