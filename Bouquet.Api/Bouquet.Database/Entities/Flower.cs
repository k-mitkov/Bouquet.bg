namespace Bouquet.Database.Entities
{
    public class Flower
    {
        public string Id { get; set; }

        public string Name { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Букети
        /// </summary>
        public IEnumerable<Bouquet>? Bouquets { get; set; }

        #endregion

        public Flower()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
