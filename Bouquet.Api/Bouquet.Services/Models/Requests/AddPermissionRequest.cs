using System.ComponentModel.DataAnnotations;

namespace Bouquet.Services.Models.Requests
{
    public class AddPermissionRequest
    {
        [Required]
        public string Role { get; set; } = string.Empty;

        [Required]
        public string Permission { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Group { get; set; } = string.Empty;   
    }
}
