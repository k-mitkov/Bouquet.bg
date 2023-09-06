using Bouquet.Services.Models.DTOs.Bouquet;
using System.ComponentModel.DataAnnotations;

namespace Bouquet.Services.Models.Requests
{
    public class MakeOrderRequest
    {
        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? ShopId { get; set; }

        public string? ReciverName { get; set; }

        public string? ReciverPhoneNumber { get; set; }

        [Required]
        public bool HasDelivery { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }

        public string? PreferredTime { get; set; }

        public decimal Price { get; set; }

        /// <summary>
        /// Цветя
        /// </summary>
        [Required]
        public IEnumerable<CartBouquetDTO>? Bouquets { get; set; }
    }
}
