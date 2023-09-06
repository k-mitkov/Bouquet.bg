using AutoMapper;
using Bouquet.Database;
using Bouquet.Database.Entities;
using Bouquet.Services.Interfaces.Bouquet;
using Bouquet.Services.Models.DTOs.Bouquet;
using Bouquet.Services.Models.Requests;
using Bouquet.Services.Models.Responses;
using Bouquet.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Bouquet.Services.Bouquet
{
    public class BouquetService : IBouquetService
    {
        #region Declarations

        private readonly BouquetContext _dbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public BouquetService(BouquetContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Взима всички букети в даден град
        /// </summary>
        /// <returns></returns>
        public async Task<Response> GetBouquetsAsync(string cityID, string shopID)
        {
            var bouquets = await _dbContext.Bouquets.Include(b => b.Pictures).Include(b => b.Colors).Include(b => b.Flowers).Include(b => b.FlowerShop).Where(b => b.Status == 0 && (b.FlowerShopID == shopID || b.FlowerShop.CityID == cityID)).ToListAsync();

            if (bouquets == null || !bouquets.Any())
            {
                return new Response { Status = StatusEnum.Failure, Message = "Bouquets not found" };
            }

            return new Response<IEnumerable<BouquetDTO>> { Status = StatusEnum.Success, Data = _mapper.Map<IEnumerable<BouquetDTO>>(bouquets) };
        }

        /// <summary>
        /// Добавя нов букет
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response> AddBouquetsAsync(AddBouquetRequest request)
        {
            try
            {
                var bouquet = _mapper.Map<Database.Entities.Bouquet>(request);

                var flowers = await _dbContext.Flowers.Where(f => bouquet.Flowers.Select(f => f.Id).Contains(f.Id)).ToListAsync();
                var colors = await _dbContext.Colors.Where(c => bouquet.Colors.Select(c => c.Id).Contains(c.Id)).ToListAsync();

                bouquet.Flowers = flowers;
                bouquet.Colors = colors;

                await _dbContext.Bouquets.AddAsync(bouquet);

                await _dbContext.SaveChangesAsync();

                return new Response<string> { Status = StatusEnum.Success, Data = bouquet.Id };

            }
            catch (Exception ex)
            {
                return new Response { Status = StatusEnum.Failure, Message = ex.Message };
            }
        }

        /// <summary>
        /// Качва снимка на букета
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public async Task<Response> UploadPictureAsync(IFormFile file, string bouquetID)
        {
            try
            {
                var bouquet = await _dbContext.Bouquets.Include(b => b.Pictures).FirstOrDefaultAsync(o => o.Id == bouquetID);

                if (bouquet == null)
                    return new Response { Status = StatusEnum.Failure, Message = "Bouquet not found" };

                string directoryPath = "Files/Bouquet-Pictures";

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var extension = Path.GetExtension(file.FileName);

                string uniqueFileName = $"{bouquetID}-{file.FileName}{extension}";
                string filePath = Path.Combine(directoryPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var pictures = new List<Picture>();

                pictures.AddRange(bouquet.Pictures);
                pictures.Add(new Picture { PictureDataUrl = filePath });

                bouquet.Pictures = pictures;
                _dbContext.Bouquets.Update(bouquet);
                await _dbContext.SaveChangesAsync();

                return new Response<string> { Status = StatusEnum.Success, Data = filePath };
            }
            catch (Exception ex)
            {
                return new Response { Status = StatusEnum.Failure, Message = "The picture is not saved." };
            }

        }

        #endregion
    }
}
