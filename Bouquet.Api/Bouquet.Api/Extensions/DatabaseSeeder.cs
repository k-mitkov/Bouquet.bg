using Bouquet.Database;
using Bouquet.Database.Entities;
using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Interfaces.User;
using Bouquet.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bouquet.Api.Extensions
{
    public class DatabaseSeeder
    {
        private readonly RoleManager<BouquetRole> _roleManager;
        private readonly BouquetContext _dbContext;
        private readonly UserManager<BouquetUser> _userManager;
        private readonly IUserService _userService;

        public DatabaseSeeder(RoleManager<BouquetRole> roleManager, BouquetContext dbContext, UserManager<BouquetUser> userManager, IUserService userService)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
            _userManager = userManager;
            _userService = userService;
        }

        /// <summary>
        /// Seeds roles
        /// </summary>
        /// <param name="roleManager"></param>
        /// <returns></returns>
        public async Task SeedRoles()
        {

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new BouquetRole()
                {
                    Name = "Admin",
                    Description="Admin role",
                    CreatedBy="admin",
                    LastModifiedBy="admin",
                    CreatedOn=DateTime.UtcNow,
                };

                await _roleManager.CreateAsync(adminRole);

                await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
                {
                    RoleId = adminRole.Id,
                    ClaimType = RoleClaimType.Permission,
                    ClaimValue = RoleClaim.AddBouquet,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Description = "Add Bouquet",
                    Group = "Bouquet"
                });

                await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
                {
                    RoleId = adminRole.Id,
                    ClaimType = RoleClaimType.Permission,
                    ClaimValue = RoleClaim.AddFlowerShop,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Description = "Add Shop",
                    Group = "Shop"
                });

                await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
                {
                    RoleId = adminRole.Id,
                    ClaimType = RoleClaimType.Permission,
                    ClaimValue = RoleClaim.AddPermission,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Description = "Add Permission",
                    Group = "Permissions"
                });


                await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
                {
                    RoleId = adminRole.Id,
                    ClaimType = RoleClaimType.Permission,
                    ClaimValue = RoleClaim.RemovePermission,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Description = "Remove Permission",
                    Group = "Permissions"
                });

                await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
                {
                    RoleId = adminRole.Id,
                    ClaimType = RoleClaimType.Permission,
                    ClaimValue = RoleClaim.AddWorker,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Description = "Add Worker",
                    Group = "Permissions"
                });

                await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
                {
                    RoleId = adminRole.Id,
                    ClaimType = RoleClaimType.Permission,
                    ClaimValue = RoleClaim.ManageOrders,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Description = "Manage Orders",
                    Group = "Permissions"
                });

                var uniqueNumber = await _userService.GenerateUserUniqueNumber("krasi0m0@gmail.com", "Admin");

                var user = new BouquetUser
                {
                    UserName = "krasi0m0@gmail.com",
                    Email = "krasi0m0@gmail.com",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    UniqueNumber = uniqueNumber,
                    UserInfo = new UserInfo()
                };

                IdentityResult result = await _userManager.CreateAsync(user, "Buket6380!");

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Admin").Wait();
                }

                await _dbContext.SaveChangesAsync();
            }

            if (!await _roleManager.RoleExistsAsync("Partner"))
            {
                var partner = new BouquetRole()
                {
                    Name = "Partner",
                    Description = "Partner role",
                    CreatedBy = "admin",
                    LastModifiedBy = "admin",
                    CreatedOn = DateTime.UtcNow,
                };

                await _roleManager.CreateAsync(partner);

                await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
                {
                    RoleId = partner.Id,
                    ClaimType = RoleClaimType.Permission,
                    ClaimValue = RoleClaim.AddBouquet,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Description = "Add Bouquet",
                    Group = "Bouquet"
                });

                await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
                {
                    RoleId = partner.Id,
                    ClaimType = RoleClaimType.Permission,
                    ClaimValue = RoleClaim.AddFlowerShop,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Description = "Add Shop",
                    Group = "Shop"
                });

                await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
                {
                    RoleId = partner.Id,
                    ClaimType = RoleClaimType.Permission,
                    ClaimValue = RoleClaim.AddWorker,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Description = "Add Worker",
                    Group = "Permissions"
                });

                await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
                {
                    RoleId = partner.Id,
                    ClaimType = RoleClaimType.Permission,
                    ClaimValue = RoleClaim.ManageOrders,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Description = "Manage Orders",
                    Group = "Permissions"
                });

                var uniqueNumber = await _userService.GenerateUserUniqueNumber("partner@gmail.com", "Partner");

                var user = new BouquetUser
                {
                    UserName = "partner@gmail.com",
                    Email = "partner@gmail.com",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    UniqueNumber = uniqueNumber,
                    UserInfo = new UserInfo()
                };

                IdentityResult result = await _userManager.CreateAsync(user, "Buket6380!");

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Partner").Wait();
                }
            }

            if (!await _roleManager.RoleExistsAsync("Worker"))
            {
                var worker = new BouquetRole()
                {
                    Name = "Worker",
                    Description = "Worker role",
                    CreatedBy = "admin",
                    LastModifiedBy="admin",
                    CreatedOn=DateTime.UtcNow,
                };

                await _roleManager.CreateAsync(worker);

                await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
                {
                    RoleId = worker.Id,
                    ClaimType = RoleClaimType.Permission,
                    ClaimValue = RoleClaim.AddBouquet,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Description = "Add Bouquet",
                    Group = "Bouquet"
                });

                await _dbContext.RoleClaims.AddAsync(new BouquetRoleClaim
                {
                    RoleId = worker.Id,
                    ClaimType = RoleClaimType.Permission,
                    ClaimValue = RoleClaim.ManageOrders,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    Description = "Manage Orders",
                    Group = "Permissions"
                });

                var uniqueNumber = await _userService.GenerateUserUniqueNumber("worker@gmail.com", "Worker");

                var user = new BouquetUser
                {
                    UserName = "worker@gmail.com",
                    Email = "worker@gmail.com",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    UniqueNumber = uniqueNumber,
                    UserInfo = new UserInfo()
                };

                IdentityResult result = await _userManager.CreateAsync(user, "Buket6380!");

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Worker").Wait();
                }

                await _dbContext.SaveChangesAsync();
            }
            

            if (!await _roleManager.RoleExistsAsync("Client"))
            {
                var client = new BouquetRole()
                {
                    Name = "Client",
                    Description = "Client role",
                    CreatedBy = "admin",
                    LastModifiedBy = "admin",
                    CreatedOn=DateTime.UtcNow,
                };
                await _roleManager.CreateAsync(client);

                var uniqueNumber = await _userService.GenerateUserUniqueNumber("partner@gmail.com", Role.Client);

                var user = new BouquetUser
                {
                    Id = "defaouth",
                    UserName = "defaouth@gmail.com",
                    Email = "defaouth@gmail.com",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    UniqueNumber = uniqueNumber,
                    UserInfo = new UserInfo()
                };

                IdentityResult result = await _userManager.CreateAsync(user, "Buket6380!");

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, Role.Client).Wait();
                }

                var wallet = new Wallet() { 
                    OwnerId = user.Id,
                    Balance = 0,
                };

                _dbContext.Wallets.Add(wallet);

                await _dbContext.SaveChangesAsync();
            }

            if(!await _dbContext.Cites.AnyAsync())
            {
                var defaultCity = new City()
                {
                    Name = "default",
                    Latitude = 42.695144,
                    Longitude = 23.329273
                };

                var sofia = new City()
                {
                    Name = "Sofia",
                    Latitude = 42.695144,
                    Longitude = 23.329273
                };

                var plovdiv = new City()
                {
                    Name = "Plovdiv",
                    Latitude = 42.138964,
                    Longitude = 24.744664
                };

                var varna = new City()
                {
                    Name = "Varna",
                    Latitude = 43.207287,
                    Longitude = 27.914054
                };

                var burgas = new City()
                {
                    Name = "Burgas",
                    Latitude = 42.494640,
                    Longitude = 27.474873
                };

                var staraZagora = new City()
                {
                    Name = "Stara Zagora",
                    Latitude = 42.425516,
                    Longitude = 25.634532
                };

                await _dbContext.Cites.AddAsync(defaultCity);
                await _dbContext.Cites.AddAsync(sofia);
                await _dbContext.Cites.AddAsync(plovdiv);
                await _dbContext.Cites.AddAsync(varna);
                await _dbContext.Cites.AddAsync(burgas);
                await _dbContext.Cites.AddAsync(staraZagora);

                await _dbContext.SaveChangesAsync();
            }

            if (!await _dbContext.Colors.AnyAsync())
            {
                var red = new Color()
                {
                    Name = "Red",
                    HexCode = "#FF0000"
                };

                var white = new Color()
                {
                    Name = "White",
                    HexCode = "#FFFFFF"
                };

                var blue = new Color()
                {
                    Name = "Blue",
                    HexCode = "#0000FF"
                };

                var black = new Color()
                {
                    Name = "Black",
                    HexCode = "#000000"
                };

                var yellow = new Color()
                {
                    Name = "Yellow ",
                    HexCode = "#FFFF00"
                };

                await _dbContext.Colors.AddAsync(red);
                await _dbContext.Colors.AddAsync(white);
                await _dbContext.Colors.AddAsync(blue);
                await _dbContext.Colors.AddAsync(black);
                await _dbContext.Colors.AddAsync(yellow);

                await _dbContext.SaveChangesAsync();
            }

            if (!await _dbContext.Flowers.AnyAsync())
            {
                var red = new Flower()
                {
                    Name = "Rose",
                };

                var white = new Flower()
                {
                    Name = "Begonia",
                };

                await _dbContext.Flowers.AddAsync(red);
                await _dbContext.Flowers.AddAsync(white);

                await _dbContext.SaveChangesAsync();
            }

            if (!await _dbContext.Agreements.AnyAsync())
            {
                var first = new Agreement()
                {
                    Name = "First partners",
                    Percent = 6.5,
                    MinFee = 1
                };

                var standart = new Agreement()
                {
                    Name = "Standart",
                    Percent = 8,
                    MinFee = 1.2
                };

                await _dbContext.Agreements.AddAsync(first);
                await _dbContext.Agreements.AddAsync(standart);

                await _dbContext.SaveChangesAsync();
            }

            if (!await _dbContext.FlowerShops.AnyAsync())
            {
                var config = new ShopConfig()
                {
                    OpenAt = TimeSpan.FromHours(8),
                    CloseAt = TimeSpan.FromHours(20),
                    FreeDeliveryAt = 30,
                    Price = 5,
                    SameDayTillHour = TimeSpan.FromHours(14),
                };

                var config2 = new ShopConfig()
                {
                    OpenAt = TimeSpan.FromHours(9),
                    CloseAt = TimeSpan.FromHours(21),
                    FreeDeliveryAt = 30,
                    Price = 5,
                    SameDayTillHour = TimeSpan.FromHours(15),
                };

                var agreement = await _dbContext.Agreements.FirstOrDefaultAsync();
                var city = await _dbContext.Cites.FirstOrDefaultAsync();

                var owner = (await _userManager.GetUsersInRoleAsync("Partner")).FirstOrDefault();
                var worker = (await _userManager.GetUsersInRoleAsync("Worker")).FirstOrDefault();

                var object1 = new FlowerShop()
                {
                    Name = "ASSD",
                    Latitude = 42.674345,
                    Longitude = 23.250016,
                    Status = 1,
                    ShopConfig = config,
                    Agreement = agreement,
                    Owner = owner,
                    Workers = new List<BouquetUser> { worker },
                    City = city,
                    Address = "asdasd"
                };

                var object2 = new FlowerShop()
                {
                    Name = "59 блок",
                    Latitude = 42.642451,
                    Longitude = 23.339126,
                    Status = 1,
                    ShopConfig = config2,
                    Agreement = agreement,
                    Owner = owner,
                    Workers = new List<BouquetUser> { worker },
                    City = city,
                    Address = "asdasd"
                };

                await _dbContext.FlowerShops.AddAsync(object1);
                await _dbContext.FlowerShops.AddAsync(object2);


                config.FlowerShopID = object1.Id;
                config2.FlowerShopID = object2.Id;

                await _dbContext.ShopConfigs.AddAsync(config);
                await _dbContext.ShopConfigs.AddAsync(config2);

                await _dbContext.SaveChangesAsync();
            }

            if (!await _dbContext.Bouquets.AnyAsync())
            {
                var flower = await _dbContext.Flowers.ToListAsync();
                var color = await _dbContext.Colors.ToListAsync();

                var bouquet = new Bouquet.Database.Entities.Bouquet()
                {
                    Name = "Summer",
                    Height = 22,
                    Price = (decimal)25,
                    FlowersCount = 5,
                    FlowerShopID = (await _dbContext.FlowerShops.FirstOrDefaultAsync()).Id,
                    Status = 1,
                    Flowers = flower,
                    Colors = color,
                    Description = "firstOne",
                };

                var bouquet2 = new Bouquet.Database.Entities.Bouquet()
                {
                    Name = "Огън",
                    Height = 35,
                    Price = (decimal)55,
                    FlowersCount = 20,
                    FlowerShopID = (await _dbContext.FlowerShops.FirstOrDefaultAsync()).Id,
                    Status = 1,
                    Flowers = flower,
                    Colors = color,
                    Description = "secondOne",
                };

                await _dbContext.Bouquets.AddAsync(bouquet);
                await _dbContext.Bouquets.AddAsync(bouquet2);

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
