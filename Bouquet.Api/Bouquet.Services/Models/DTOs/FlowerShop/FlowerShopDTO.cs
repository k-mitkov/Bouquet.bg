namespace Bouquet.Services.Models.DTOs.FlowerShop
{
    public class FlowerShopDTO
    {
        public string? Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? PictureDataUrl { get; set; }

        public string? OwnerId { get; set; }

        public string? City { get; set; }

        public ShopConfigDTO? ShopConfig { get; set; }

        public IEnumerable<string>? Workers { get; set; }
    }
}
