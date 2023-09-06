namespace Bouquet.Services.Models.Authentication
{
    public class LoginResponseModel : JWTTokenModel
    {
        public string ID { get; set; } = string.Empty;

        public string ProfilePictureUrl { get; set; } = string.Empty;

        public IEnumerable<string> Claims { get; set; } = default!;
    }
}
