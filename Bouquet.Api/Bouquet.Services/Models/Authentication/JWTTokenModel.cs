namespace Bouquet.Services.Models.Authentication
{
    public class JWTTokenModel
    {
        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }
    }
}
