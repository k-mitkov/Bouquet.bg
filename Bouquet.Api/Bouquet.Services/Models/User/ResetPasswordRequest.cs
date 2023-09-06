using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bouquet.Services.Models.User
{
    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; } = default!;

        [Required]
        [JsonPropertyName("password")]
        public string NewPassword { get; set; } = default!;

        [Required]
        [JsonPropertyName("token")]
        public string Token { get; set; } = default!;
    }
}
