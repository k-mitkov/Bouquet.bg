using Bouquet.Database.Entities.Payments;
using Bouquet.Database.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bouquet.Database.Entities.Identity
{
    public class BouquetUser : IdentityUser, IAuditableEntity
    {
        public string UniqueNumber { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string? ProfilePictureDataUrl { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool? IsActive { get; set; } = true;

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public string? WalletID { get; set; }

        public string? CutomerId { get; set; }

        #region Navigation database properties

        public UserInfo UserInfo { get; set; } = default!;

        public Wallet? Wallet { get; set; } = default!;

        public IEnumerable<FlowerShop>? WorkPlaces { get; set; } = default!;

        public IEnumerable<FlowerShop>? OwnedShops { get; set; } = default!;

        public IEnumerable<Order>? Orders { get; set; } = default!;

        public IEnumerable<DeliveryDetails>? DeliveryDetails { get; set; } = default!;

        public IEnumerable<Card>? Cards { get; set; } = default!;

        #endregion
    }
}
