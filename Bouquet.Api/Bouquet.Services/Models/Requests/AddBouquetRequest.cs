using Bouquet.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace Bouquet.Services.Models.Requests
{
    public class AddBouquetRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Height { get; set; }

        [Required]
        public int Width { get; set; }

        [Required]
        public int FlowersCount { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        [Required]
        public string? FlowerShopID { get; set; }

        /// <summary>
        /// Цветя
        /// </summary>
        [Required]
        public IEnumerable<Flower>? Flowers { get; set; }

        /// <summary>
        /// Цветове
        /// </summary>
        [Required]
        public IEnumerable<Color>? Colors { get; set; }
    }
}
