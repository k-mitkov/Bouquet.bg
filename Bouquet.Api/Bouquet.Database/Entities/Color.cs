namespace Bouquet.Database.Entities
{
    public class Color
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string HexCode { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Букети
        /// </summary>
        public IEnumerable<Bouquet>? Bouquets { get; set; }

        #endregion

        public Color()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
