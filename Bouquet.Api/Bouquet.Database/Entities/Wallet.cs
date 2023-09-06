using Bouquet.Database.Entities.Identity;

namespace Bouquet.Database.Entities
{
    public class Wallet
    {
        public string Id { get; set; }

        public decimal Balance { get; set; }

        public string OwnerId { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Собсвеник
        /// </summary>
        public BouquetUser? Owner { get; set; }

        /// <summary>
        /// Транзакции
        /// </summary>
        public IEnumerable<Transaction>? TransactionsFrom { get; set; }

        /// <summary>
        /// Транзакции
        /// </summary>
        public IEnumerable<Transaction>? TransactionsTo { get; set; }

        #endregion

        public Wallet()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
