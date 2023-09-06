using System.ComponentModel.DataAnnotations;

namespace Bouquet.Services.Models.Requests
{
    public class AddWorkerRequest
    {
        [Required]
        public string? UserUniqueNumber { get; set; }

        [Required]
        public string? FlowerShopID { get; set; }
    }
}
