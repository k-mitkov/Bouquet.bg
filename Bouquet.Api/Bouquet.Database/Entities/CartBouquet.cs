namespace Bouquet.Database.Entities
{
    public class CartBouquet
    {
        public string Id { get; set; }

        public int Count { get; set; }

        public string BouquetID { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Букет
        /// </summary>
        public Bouquet? Bouquet { get; set; }

        #endregion

        public CartBouquet()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
