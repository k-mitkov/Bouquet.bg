using Bouquet.Database.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Bouquet.Database.Entities.Identity
{
    public class BouquetRole : IdentityRole, IAuditableEntity
    {
        public BouquetRole()
        {
            RoleClaims = new HashSet<BouquetRoleClaim>();
        }

        public string? Description { get; set; }

        public int? Priority { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public IReadOnlyCollection<BouquetRoleClaim> RoleClaims { get; private set; }
    }
}
