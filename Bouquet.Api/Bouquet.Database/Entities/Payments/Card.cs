using Bouquet.Database.Entities.Identity;

namespace Bouquet.Database.Entities.Payments
{
    public class Card
    {
        public string Id { get; set; }

        public string Last4 { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string CardId { get; set; } = string.Empty;

        public string UserID { get; set; } = string.Empty;

        #region Navigation database properties

        public BouquetUser User { get; set; } = default!;

        public IEnumerable<Payment>? Payments { get; set; } = default!;

        #endregion

        public Card()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
