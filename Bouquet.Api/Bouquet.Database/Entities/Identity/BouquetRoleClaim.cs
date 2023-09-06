using Bouquet.Database.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Bouquet.Database.Entities.Identity
{
    public class BouquetRoleClaim : IdentityRoleClaim<string>, IAuditableEntity
    {
        public BouquetRoleClaim()
        {
        }

        public BouquetRoleClaim(string? roleClaimDescription = null, string? roleClaimGroup = null)
        {
            Description = roleClaimDescription;
            Group = roleClaimGroup;
        }

        public string? Description { get; set; }

        public string? Group { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public BouquetRole? Role { get; set; }
    }
}
