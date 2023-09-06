namespace Bouquet.Database.Entities
{
    public class Transaction
    {
        public string Id { get; set; }

        public decimal Ammount { get; set; }

        public string OrderID { get; set; }

        public string? WalletFromID { get; set; }

        public string WalletToID { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Поръчка
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// От
        /// </summary>
        public Wallet WalletFrom { get; set; }

        /// <summary>
        /// До
        /// </summary>
        public Wallet WalletTo{ get; set; }

        #endregion

        public Transaction()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
