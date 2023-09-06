using Bouquet.Database;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Bouquet.Api.Authorization
{
    public class RoleClaimsHandler : AuthorizationHandler<RoleClaimsRequirement>
    {
        private readonly BouquetContext _dbContext;

        public RoleClaimsHandler(BouquetContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// При ползването на [Authorize(Policy = Permission...)] влиза в този handler, за да се разбере дали потребителят има права
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleClaimsRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var userRole = context.User.FindFirst(ClaimTypes.Role)!.Value;

            var claims = _dbContext.RoleClaims.Where(rc => rc.Role != null && rc.Role.Name!.ToLower() == userRole.ToLower()).Select(rc => rc.ClaimValue).ToList();

            if (claims == null || claims.Count == 0)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (claims.Contains(requirement.RoleClaim))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
