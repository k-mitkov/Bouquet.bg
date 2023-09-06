using System.ComponentModel.DataAnnotations;

namespace Bouquet.Services.Models.Requests
{
    public class RemovePermissionRequest
    {
        [Required]
        public string Role { get; set; } = string.Empty;

        [Required]
        public string Permission { get; set; } = string.Empty;
    }
}
