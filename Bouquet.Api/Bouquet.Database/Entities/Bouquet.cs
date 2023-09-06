namespace Bouquet.Database.Entities
{
    public class Bouquet
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int FlowersCount { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string FlowerShopID { get; set; }

        public int Status { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Магазин
        /// </summary>
        public FlowerShop? FlowerShop { get; set; }

        /// <summary>
        /// Снимки
        /// </summary>
        public IEnumerable<Picture>? Pictures { get; set; }

        /// <summary>
        /// Цветя
        /// </summary>
        public IEnumerable<Flower>? Flowers { get; set; }

        /// <summary>
        /// Цветове
        /// </summary>
        public IEnumerable<Color>? Colors { get; set; }

        /// <summary>
        /// Поръчки букети
        /// </summary>
        public IEnumerable<CartBouquet>? CartBouquets { get; set; }

        #endregion

        public Bouquet()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
