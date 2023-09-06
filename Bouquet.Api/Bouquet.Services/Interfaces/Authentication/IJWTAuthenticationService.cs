using Bouquet.Services.Models.Authentication;
using Bouquet.Services.Models.Responses;
using Bouquet.Services.Models.User;

namespace Bouquet.Services.Interfaces.Authentication
{
    public interface IJWTAuthenticationService
    {
        Task<Response> Login(LoginModel loginModel);

        Task<Response> Register(RegisterInfo registerModel);

        Task<Response> RefreshToken(JWTTokenModel tokenModel);
    }

}
