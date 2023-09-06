using Microsoft.AspNetCore.Authorization;

namespace Bouquet.Api.Authorization
{
    public class RoleClaimsRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Използва се в Handler-ът - проверява се дали потребителят има подаденият Permission
        /// </summary>
        public string RoleClaim { get; set; } = string.Empty;

        public RoleClaimsRequirement(string roleClaim)
        {
            RoleClaim = roleClaim;
        }
    }
}
