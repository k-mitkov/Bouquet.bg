using Bouquet.Database.Entities.Identity;

namespace Bouquet.Database.Entities
{
    public class Order
    {
        public string Id { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string FlowerShopID { get; set; }

        public string? UserID { get; set; }

        public string? AnonymousCustomerID { get; set; }

        public string DeliveryDetailsID { get; set; }

        public string OrderCartID { get; set; }

        public int Status { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Доставка
        /// </summary>
        public DeliveryDetails? DeliveryDetails { get; set; }

        /// <summary>
        /// Магазин
        /// </summary>
        public FlowerShop? FlowerShop { get; set; }

        /// <summary>
        /// Количка
        /// </summary>
        public OrderCart? OrderCart { get; set; }

        /// <summary>
        /// Потребител
        /// </summary>
        public BouquetUser? User { get; set; }

        /// <summary>
        /// Анонимен Потребител
        /// </summary>
        public AnonymousCustomer? AnonymousCustomer { get; set; }

        /// <summary>
        /// Транзакции
        /// </summary>
        public IEnumerable<Transaction>? Transactions { get; set; }

        #endregion

        public Order()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
