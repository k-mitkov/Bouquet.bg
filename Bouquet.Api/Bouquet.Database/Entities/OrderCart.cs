namespace Bouquet.Database.Entities
{
    public class OrderCart
    {
        public string Id { get; set; }

        public string OrderID { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Поръчка
        /// </summary>
        public Order? Order { get; set; }

        /// <summary>
        /// Букети
        /// </summary>
        public IEnumerable<CartBouquet>? Bouquets { get; set; }

        #endregion

        public OrderCart()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
