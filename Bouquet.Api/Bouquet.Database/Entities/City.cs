using System.ComponentModel.DataAnnotations;

namespace Bouquet.Database.Entities
{
    public class City
    {
        public string Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Цветарници в града
        /// </summary>
        public IEnumerable<FlowerShop>? FlowerShops { get; set; }

        #endregion

        public City()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
