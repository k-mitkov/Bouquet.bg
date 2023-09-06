using System.ComponentModel.DataAnnotations;

namespace Bouquet.Database.Entities.Identity
{
    public class UserInfo
    {
        public string Id { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(70)]
        public string? MOL { get; set; }

        [StringLength(56)]
        public string? Country { get; set; }

        [StringLength(35)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? TaxId { get; set; }

        [StringLength(100)]
        public string? VatId { get; set; }

        public string? Company { get; set; }

        public string UserId { get; set; } = string.Empty;

        public BouquetUser User { get; set; } = default!;

        public UserInfo()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
