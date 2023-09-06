using Bouquet.Database.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bouquet.Database.Entities
{
    public class DeliveryDetails
    {
        public string Id { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string ReciverName { get; set; }

        public DateTime PreferredTime { get; set; }

        public string? UserID { get; set; }

        public int Type { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Поръчки
        /// </summary>
        public IEnumerable<Order>? Orders { get; set; }

        /// <summary>
        /// Потребител
        /// </summary>
        public BouquetUser? User { get; set; }

        #endregion

        public DeliveryDetails()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
