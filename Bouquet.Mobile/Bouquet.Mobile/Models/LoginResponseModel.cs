using System.Collections.Generic;

namespace Bouquet.Mobile.Models
{
    public class LoginResponseModel : JWTTokenModel
    {
        public string ID { get; set; } = string.Empty;

        public string ProfilePictureUrl { get; set; } = string.Empty;

        public IEnumerable<string> Claims { get; set; } = default;
    }
}
