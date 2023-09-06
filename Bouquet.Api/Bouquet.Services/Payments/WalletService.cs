using Bouquet.Database;
using Bouquet.Database.Entities;
using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Interfaces.Payment;
using Bouquet.Services.Models.Requests;
using Bouquet.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bouquet.Services.Payments
{
    public class WalletService : IWalletService
    {
        private readonly BouquetContext _dbContext;
        private readonly ILogger<WalletService> _logger;
        private readonly UserManager<BouquetUser> _userManager;

        public WalletService(BouquetContext dbContext, ILogger<WalletService> logger,
                                UserManager<BouquetUser> userManager)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task HandlePayment(CreatePaymentRequestBase payment)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == payment.OrderId);

                if (order == null)
                {
                    _logger.LogError("Order not found");
                }

                var shop = await _dbContext.FlowerShops.Include(s => s.Owner).ThenInclude(u => u.Wallet).Include(s => s.Agreement).FirstOrDefaultAsync(s => s.Id == order.FlowerShopID);

                if (shop == null)
                {
                    _logger.LogError("Shop not found");
                }

                var owner = shop.Owner;

                if(owner.Wallet ==  null)
                {
                    owner.Wallet = new Wallet() {
                        OwnerId = owner.Id,
                        Balance = 0
                    };
                }

                var admin = (await _userManager.GetUsersInRoleAsync(Role.Admin)).OrderBy(u => u.UniqueNumber).FirstOrDefault();

                if (admin.Wallet == null)
                {
                    admin.Wallet = new Wallet()
                    {
                        OwnerId = admin.Id,
                        Balance = 0
                    };
                }

                var tax = Math.Round(order.Price * (decimal)shop.Agreement.Percent / 100, 2);

                tax = tax < (decimal)shop.Agreement.MinFee ? (decimal)shop.Agreement.MinFee : tax;

                var walletFrom = await _dbContext.Wallets.FirstOrDefaultAsync(w => w.OwnerId == "defaouth"); 

                var transactionsToOwner = owner.Wallet.TransactionsTo != null ? owner.Wallet.TransactionsTo.ToList() : new List<Transaction>();
                owner.Wallet.Balance = order.Price - tax;
                transactionsToOwner.Add(new Transaction()
                {
                    OrderID = order.Id,
                    Ammount = order.Price - tax,
                    WalletToID = owner.Wallet.Id,
                    WalletFromID = walletFrom.Id,
                });
                owner.Wallet.TransactionsTo = transactionsToOwner;

                var transactionsToAdmin = admin.Wallet.TransactionsTo != null ? admin.Wallet.TransactionsTo.ToList() : new List<Transaction>();
                admin.Wallet.Balance = tax;
                transactionsToAdmin.Add(new Transaction()
                {
                    OrderID = order.Id,
                    Ammount = tax,
                    WalletToID = admin.Wallet.Id,
                    WalletFromID = walletFrom.Id,
                });
                admin.Wallet.TransactionsTo = transactionsToAdmin;

                _dbContext.Update(owner);
                _dbContext.Update(admin);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
