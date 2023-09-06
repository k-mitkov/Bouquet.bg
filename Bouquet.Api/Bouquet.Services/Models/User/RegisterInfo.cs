using Bouquet.Services.Models.Authentication;
using Bouquet.Services.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Bouquet.Services.Models.User
{
    public class RegisterInfo
    {
        [Required]
        public UserInfoDTO UserInfo { get; set; } = default!;

        [Required]
        public LoginModel UserCredentials { get; set; } = default!;
    }
}
