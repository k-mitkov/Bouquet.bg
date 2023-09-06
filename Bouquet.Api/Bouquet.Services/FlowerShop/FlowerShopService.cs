using AutoMapper;
using Bouquet.Database;
using Bouquet.Database.Entities;
using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Interfaces.FlowerShop;
using Bouquet.Services.Models.DTOs;
using Bouquet.Services.Models.DTOs.FlowerShop;
using Bouquet.Services.Models.Requests;
using Bouquet.Services.Models.Responses;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Bouquet.Services.FlowerShop
{
    public class FlowerShopService : IFlowerShopService
    {
        #region Declarations

        private readonly BouquetContext _dbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public FlowerShopService(BouquetContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Взима всички обекти
        /// </summary>
        /// <returns></returns>
        public async Task<Response> GetShopsAsync()
        {
            var shops = await _dbContext.FlowerShops.Include(s => s.ShopConfig).Include(s => s.Workers).Include(s=>s.City).ToListAsync();

            if (shops == null || !shops.Any())
            {
                return new Response { Status = StatusEnum.Failure, Message = "Shops not found" };
            }

            return new Response<IEnumerable<FlowerShopDTO>> { Status = StatusEnum.Success, Data = _mapper.Map<IEnumerable<FlowerShopDTO>>(shops) };
        }

        /// <summary>
        /// Взима обект
        /// </summary>
        /// <returns></returns>
        public async Task<Response> GetShopAsync(string shopId)
        {
            var shops = await _dbContext.FlowerShops.Include(s => s.ShopConfig).Include(s => s.Workers).Include(s => s.City).FirstOrDefaultAsync(s => s.Id == shopId);

            if (shops == null)
            {
                return new Response { Status = StatusEnum.Failure, Message = "Shop not found" };
            }

            return new Response<FlowerShopDTO> { Status = StatusEnum.Success, Data = _mapper.Map<FlowerShopDTO>(shops) };
        }

        /// <summary>
        /// Взима обектите собственост на партньора
        /// </summary>
        /// <returns></returns>
        public async Task<Response> GetOwnedShopsAsync(string email)
        {
            var user = await _dbContext.Users.Include(u => u.OwnedShops).ThenInclude(s => s.City).FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());

            if (user == null)
                return new Response { Status = StatusEnum.Failure, Message = "User not found" };

            var shops = user.OwnedShops;

            if (shops == null || !shops.Any())
            {
                return new Response { Status = StatusEnum.Failure, Message = "Shops not found" };
            }

            return new Response<IEnumerable<FlowerShopDTO>> { Status = StatusEnum.Success, Data = _mapper.Map<IEnumerable<FlowerShopDTO>>(shops) };
        }

        // <summary>
        /// Взима обекти в които потребителя има право да работи
        /// </summary>
        /// <returns></returns>
        public async Task<Response> GetWorkPlacesAsync(string email)
        {
            var user = await _dbContext.Users.Include(u => u.OwnedShops).ThenInclude(s => s.City).Include(u => u.WorkPlaces).ThenInclude(s => s.City).FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());

            if (user == null)
                return new Response { Status = StatusEnum.Failure, Message = "User not found" };

            var workPlaces = user.WorkPlaces ?? new List<Database.Entities.FlowerShop>();
            var ownedShops = user.OwnedShops ?? new List<Database.Entities.FlowerShop>();
            var shops = workPlaces.ToList();
            shops.AddRange(ownedShops);

            if (shops == null || !shops.Any())
            {
                return new Response { Status = StatusEnum.Failure, Message = "Shops not found" };
            }

            return new Response<IEnumerable<FlowerShopDTO>> { Status = StatusEnum.Success, Data = _mapper.Map<IEnumerable<FlowerShopDTO>>(shops) };
        }

        /// <summary>
        /// Взима служителите в обект
        /// </summary>
        /// <returns></returns>
        public async Task<Response> GetWorkersAsync(string shopID, string email)
        {
            var user = await _dbContext.Users.Include(u => u.OwnedShops).ThenInclude(s => s.Workers).ThenInclude(u => u.UserInfo).FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());

            if (user == null)
                return new Response { Status = StatusEnum.Failure, Message = "User not found" };

            var shops = user.OwnedShops;

            if (shops == null)
            {
                return new Response { Status = StatusEnum.Failure, Message = "Shop not found" };
            }

            var shop = shops.FirstOrDefault(s => s.Id == shopID);

            if (shop == null)
            {
                return new Response { Status = StatusEnum.Failure, Message = "Shop not found" };
            }

            return new Response<IEnumerable<WorkerDTO>> { Status = StatusEnum.Success, Data = _mapper.Map<IEnumerable<WorkerDTO>>(shop.Workers) };
        }

        /// <summary>
        /// Връща url за снимка на обекта
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response> GetPictureUrlAsync(string shopID)
        {
            var profilePictureUrl = await _dbContext.FlowerShops.Where(o => o.Id == shopID).Select(o => o.PictureDataUrl).FirstOrDefaultAsync();

            return new Response<string> { Status = StatusEnum.Success, Data = profilePictureUrl };
        }

        /// <summary>
        /// Мейли отговарящи на даден обект
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetWorkersEmails(string shopId)
        {
            var shop = _dbContext.FlowerShops.Include(s => s.Workers).Include(s => s.Owner).FirstOrDefault(s => s.Id == shopId);

            var emails = shop.Workers.Select(w => w.Email).ToList();

            emails.Add(shop.Owner.Email);

            return emails;
        }

        /// <summary>
        /// Добавяне на обект
        /// </summary>
        /// <returns></returns>
        public async Task<Response> AddShopAsync(AddFlowerShopRequest shopRequest, string email)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());

                if (user == null)
                    return new Response { Status = StatusEnum.Failure, Message = "User not found" };

                var flowerShop = _mapper.Map<Database.Entities.FlowerShop>(shopRequest);
                var shopConfig = _mapper.Map<ShopConfig>(shopRequest);

                if (flowerShop.CityID.Equals("default"))
                {
                    var city = await _dbContext.Cites.FirstOrDefaultAsync(u => u.Name.Equals("default"));

                    if (city == null)
                        return new Response { Status = StatusEnum.Failure, Message = "City not found" };

                    flowerShop.CityID = city.Id;
                }

                flowerShop.AgreementID = (await _dbContext.Agreements.FirstOrDefaultAsync(a => a.Name.Equals("Standart")))!.Id;
                flowerShop.OwnerID = user.Id;
                flowerShop.ShopConfig = shopConfig;

                await _dbContext.FlowerShops.AddAsync(flowerShop);
                shopConfig.FlowerShopID = flowerShop.Id;
                await _dbContext.ShopConfigs.AddAsync(shopConfig);

                await _dbContext.SaveChangesAsync();

                return new Response<string> { Status = StatusEnum.Success, Data = flowerShop.Id };

            }
            catch (Exception ex)
            {
                return new Response { Status = StatusEnum.Failure, Message = ex.Message };
            }
        }

        /// <summary>
        /// Добавяне на служител
        /// </summary>
        /// <returns></returns>
        public async Task<Response> AddWorkerAsync(AddWorkerRequest addWorkerRequest, string email)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());

                if (user == null)
                    return new Response { Status = StatusEnum.Failure, Message = "User not found" };

                var worker = await _dbContext.Users.Include(s => s.WorkPlaces).FirstOrDefaultAsync(u => u.UniqueNumber.ToLower() == addWorkerRequest.UserUniqueNumber.ToLower());

                if (worker == null)
                    return new Response { Status = StatusEnum.Failure, Message = "User not found" };

                var shop = await _dbContext.FlowerShops.Include(s => s.Workers).FirstOrDefaultAsync(s => s.Id == addWorkerRequest.FlowerShopID);

                if (shop == null)
                    return new Response { Status = StatusEnum.Failure, Message = "Shop not found" };

                if (shop.OwnerID != user.Id)
                {
                    return new Response { Status = StatusEnum.Failure, Message = "Only owner can add workers" };
                }

                var workers = shop.Workers.ToList() ?? new List<BouquetUser>();

                var workPlaces = worker.WorkPlaces.ToList() ?? new List<Database.Entities.FlowerShop>();

                workers.Add(worker);
                workPlaces.Add(shop);

                shop.Workers = workers;
                worker.WorkPlaces = workPlaces;

                _dbContext.Update(worker);
                _dbContext.Update(shop);

                await _dbContext.SaveChangesAsync();

                return new Response<string> { Status = StatusEnum.Success };

            }
            catch (Exception ex)
            {
                return new Response { Status = StatusEnum.Failure, Message = ex.Message };
            }
        }

        /// <summary>
        /// Качва снимка за обекта
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public async Task<Response> UploadPictureAsync(IFormFile file, string objectID)
        {
            try
            {
                var shop = await _dbContext.FlowerShops.FirstOrDefaultAsync(o => o.Id == objectID);

                if (shop == null)
                    return new Response { Status = StatusEnum.Failure, Message = "Shops not found" };

                string directoryPath = "Files/Shop-Pictures";

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var extension = Path.GetExtension(file.FileName);

                string uniqueFileName = $"{objectID}-{file.FileName}{extension}";
                string filePath = Path.Combine(directoryPath, uniqueFileName);

                var filesToDelete = Directory.GetFiles(directoryPath).Where(f => f.Contains(objectID));

                foreach (var fileToDelete in filesToDelete)
                {
                    File.Delete(fileToDelete);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                shop.PictureDataUrl = filePath;
                _dbContext.FlowerShops.Update(shop);
                await _dbContext.SaveChangesAsync();

                return new Response<string> { Status = StatusEnum.Success, Data = filePath };
            }
            catch (Exception ex)
            {
                return new Response { Status = StatusEnum.Failure, Message = "The picture is not saved." };
            }

        }

        /// <summary>
        /// Премахва служител
        /// </summary>
        /// <param name="file"></param>
        /// <param name="shopID"></param>
        /// <returns></returns>
        public async Task<Response> RemoveWorkerAsync(string workerId, string objectID, string email)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());

                if (user == null)
                    return new Response { Status = StatusEnum.Failure, Message = "User not found" };

                var worker = await _dbContext.Users.Include(s => s.WorkPlaces).FirstOrDefaultAsync(u => u.Id == workerId);

                if (worker == null)
                    return new Response { Status = StatusEnum.Failure, Message = "User not found" };

                var shop = await _dbContext.FlowerShops.Include(s => s.Workers).FirstOrDefaultAsync(s => s.Id == objectID);

                if (shop == null)
                    return new Response { Status = StatusEnum.Failure, Message = "Shop not found" };

                if (shop.OwnerID != user.Id)
                {
                    return new Response { Status = StatusEnum.Failure, Message = "Only owner can add workers" };
                }

                if (!shop.Workers.Any(w => w.Id == workerId))
                {
                    return new Response { Status = StatusEnum.Failure, Message = "User is not worker at this shop" };
                }

                var workers = shop.Workers.ToList();

                var workPlaces = worker.WorkPlaces.ToList();

                workers.Remove(worker);
                workPlaces.Remove(shop);

                shop.Workers = workers;
                worker.WorkPlaces = workPlaces;

                _dbContext.Update(worker);
                _dbContext.Update(shop);

                await _dbContext.SaveChangesAsync();

                return new Response<bool> { Status = StatusEnum.Success, Data = worker.WorkPlaces.Count() > 0 };

            }
            catch (Exception ex)
            {
                return new Response { Status = StatusEnum.Failure, Message = ex.Message };
            }
        }

        #endregion
    }
}
