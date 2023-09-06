using Bouquet.Database.Entities;
using Bouquet.Database.Entities.Identity;
using Bouquet.Database.Entities.Payments;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bouquet.Database
{
    public class BouquetContext : IdentityDbContext<BouquetUser, BouquetRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, BouquetRoleClaim, IdentityUserToken<string>>
    {
        public DbSet<UserInfo> UserInfos { get; set; }

        public DbSet<Entities.Bouquet> Bouquets { get; set; }

        public DbSet<Agreement> Agreements { get; set; }

        public DbSet<AnonymousCustomer> AnonymousCustomers { get; set; }

        public DbSet<City> Cites { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<DeliveryDetails> DeliveryDetails { get; set; }

        public DbSet<Flower> Flowers { get; set; }

        public DbSet<FlowerShop> FlowerShops { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderCart> OrderCarts { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<ShopConfig> ShopConfigs { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Payment> Payments { get; set; }


        public BouquetContext(DbContextOptions<BouquetContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BouquetRoleClaim>()
                .Ignore(c => c.Role);

            builder.Entity<BouquetRoleClaim>()
            .HasOne(c => c.Role)
            .WithMany(r => r.RoleClaims)
            .HasForeignKey(c => c.RoleId);

            builder.Entity<Entities.Bouquet>()
                .HasOne(b => b.FlowerShop)
                .WithMany(f => f.Bouquets)
                .HasForeignKey(c => c.FlowerShopID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Entities.Bouquet>()
                .HasMany(b => b.Flowers)
                .WithMany(f => f.Bouquets);

            builder.Entity<Entities.Bouquet>()
                .HasMany(b => b.Colors)
                .WithMany(c => c.Bouquets);

            builder.Entity<FlowerShop>()
                .HasOne(s => s.Owner)
                .WithMany(u => u.OwnedShops)
                .HasForeignKey(s => s.OwnerID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FlowerShop>()
                .HasOne(s => s.Agreement)
                .WithMany(a => a.FlowerShops)
                .HasForeignKey(s => s.AgreementID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FlowerShop>()
                .HasOne(s => s.City)
                .WithMany(c => c.FlowerShops)
                .HasForeignKey(s => s.CityID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FlowerShop>()
                .HasOne(s => s.ShopConfig)
                .WithOne(c => c.FlowerShop)
                .HasForeignKey<FlowerShop>(s => s.ShopConfigID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FlowerShop>()
                .HasMany(s => s.Workers)
                .WithMany(u => u.WorkPlaces);

            builder.Entity<Order>()
                .HasOne(o => o.AnonymousCustomer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.AnonymousCustomerID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.DeliveryDetails)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.DeliveryDetailsID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.OrderCart)
                .WithOne(d => d.Order)
                .HasForeignKey<Order>(o => o.OrderCartID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.FlowerShop)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.FlowerShopID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DeliveryDetails>()
                .HasOne(d => d.User)
                .WithMany(u => u.DeliveryDetails)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Picture>()
                .HasOne(p => p.Bouquet)
                .WithMany(b => b.Pictures)
                .HasForeignKey(d => d.BouquetID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Transaction>()
                .HasOne(t => t.Order)
                .WithMany(o => o.Transactions)
                .HasForeignKey(t => t.OrderID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Transaction>()
                .HasOne(t => t.WalletFrom)
                .WithMany(u => u.TransactionsFrom)
                .HasForeignKey(t => t.WalletFromID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Transaction>()
                .HasOne(t => t.WalletTo)
                .WithMany(u => u.TransactionsTo)
                .HasForeignKey(t => t.WalletToID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Wallet>()
                .HasOne(w => w.Owner)
                .WithOne(u => u.Wallet)
                .HasForeignKey<BouquetUser>(o => o.WalletID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Card>()
                .HasOne(c => c.User)
                .WithMany(u => u.Cards)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Payment>()
                .HasOne(p => p.Card)
                .WithMany(c => c.Payments)
                .HasForeignKey(c => c.PaymentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<BouquetUser>().ToTable("Users");
            builder.Entity<BouquetRole>().ToTable("Roles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<BouquetRoleClaim>().ToTable("RoleClaims");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<Entities.Bouquet>().ToTable("Bouquets");
            builder.Entity<Agreement>().ToTable("Agreements");
            builder.Entity<AnonymousCustomer>().ToTable("AnonymousCustomers");
            builder.Entity<City>().ToTable("Citys");
            builder.Entity<Color>().ToTable("Colors");
            builder.Entity<DeliveryDetails>().ToTable("DeliveryDetails");
            builder.Entity<Flower>().ToTable("Flowers");
            builder.Entity<FlowerShop>().ToTable("FlowerShops");
            builder.Entity<Order>().ToTable("Orders");
            builder.Entity<OrderCart>().ToTable("OrderCarts");
            builder.Entity<Picture>().ToTable("Pictures");
            builder.Entity<ShopConfig>().ToTable("ShopConfigs");
            builder.Entity<Transaction>().ToTable("Transactions");
            builder.Entity<Wallet>().ToTable("Wallets");
            builder.Entity<Card>().ToTable("Cards");
            builder.Entity<Payment>().ToTable("Payments");
        }
    }
}
