namespace Bouquet.Database.Entities
{
    public class ShopConfig
    {
        public string Id { get; set; }

        public TimeSpan OpenAt { get; set; }

        public TimeSpan CloseAt { get; set; }

        public TimeSpan SameDayTillHour { get; set; }

        public double Price { get; set; }

        public double FreeDeliveryAt { get; set; }

        public string FlowerShopID { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Магазин
        /// </summary>
        public FlowerShop? FlowerShop { get; set; }

        #endregion

        public ShopConfig()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
