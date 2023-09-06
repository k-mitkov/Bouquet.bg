namespace Bouquet.Database.Entities.Payments
{
    public class Payment
    {
        public string Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateOfPayment { get; set; }

        public string OrderId { get; set; } = string.Empty;

        public string PaymentId { get; set; } = string.Empty;

        public string CardId { get; set; } = string.Empty;

        #region Navigation database properties

        public Card Card { get; set; } = default!;

        #endregion

        public Payment()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
