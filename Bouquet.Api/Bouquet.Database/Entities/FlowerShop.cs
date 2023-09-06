using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Bouquet.Database.Entities.Identity;

namespace Bouquet.Database.Entities
{
    public class FlowerShop
    {
        public string Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [Column(TypeName = "text")]
        public string? PictureDataUrl { get; set; }

        public string CityID { get; set; }

        public string OwnerID { get; set; }

        public string ShopConfigID { get; set; }

        public string AgreementID { get; set; }

        public int Status { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Работници
        /// </summary>
        public IEnumerable<BouquetUser>? Workers { get; set; }

        /// <summary>
        /// Работници
        /// </summary>
        public City? City { get; set; }

        /// <summary>
        /// Собственик
        /// </summary>
        public BouquetUser? Owner { get; set; }

        /// <summary>
        /// Конфигурации
        /// </summary>
        public ShopConfig? ShopConfig { get; set; }

        /// <summary>
        /// Споразумение
        /// </summary>
        public Agreement? Agreement { get; set; }

        /// <summary>
        /// Поръчки
        /// </summary>
        public IEnumerable<Order>? Orders { get; set; }

        /// <summary>
        /// Букети
        /// </summary>
        public IEnumerable<Bouquet>? Bouquets { get; set; }

        #endregion

        public FlowerShop()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
