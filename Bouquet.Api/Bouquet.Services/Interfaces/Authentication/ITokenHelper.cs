using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bouquet.Services.Interfaces.Authentication
{
    public interface ITokenHelper
    {
        JwtSecurityToken CreateToken(List<Claim> authClaims);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string? token);
    }
}
