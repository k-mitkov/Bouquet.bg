using Bouquet.Services.Models.DTOs.FlowerShop;

namespace Bouquet.Services.Models.DTOs.Bouquet
{
    public class BouquetDTO
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int FlowersCount { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string? FlowerShopID { get; set; }

        /// <summary>
        /// Магазин
        /// </summary>
        public FlowerShopDTO? FlowerShop { get; set; }

        /// <summary>
        /// Снимки
        /// </summary>
        public IEnumerable<PictureDTO>? Pictures { get; set; }

        /// <summary>
        /// Цветя
        /// </summary>
        public IEnumerable<FlowerDTO>? Flowers { get; set; }

        /// <summary>
        /// Цветове
        /// </summary>
        public IEnumerable<ColorDTO>? Colors { get; set; }
    }
}
