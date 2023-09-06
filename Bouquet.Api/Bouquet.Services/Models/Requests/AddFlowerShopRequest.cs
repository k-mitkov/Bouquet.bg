using System.ComponentModel.DataAnnotations;

namespace Bouquet.Services.Models.Requests
{
    public class AddFlowerShopRequest
    {
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? OpenAt { get; set; }

        [Required]
        public string? CloseAt { get; set; }

        [Required]
        public string? SameDayTillHour { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public double FreeDeliveryAt { get; set; }

        [Required]
        public string? CityID { get; set;}
    }
}
