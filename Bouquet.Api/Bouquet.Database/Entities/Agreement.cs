namespace Bouquet.Database.Entities
{
    public class Agreement
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Percent { get; set; }

        public double MinFee { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Магазини
        /// </summary>
        public IEnumerable<FlowerShop>? FlowerShops { get; set; }

        #endregion

        public Agreement()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
