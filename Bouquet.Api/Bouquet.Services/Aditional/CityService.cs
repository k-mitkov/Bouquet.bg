using AutoMapper;
using Bouquet.Database;
using Bouquet.Database.Entities;
using Bouquet.Services.Interfaces.Aditional;
using Bouquet.Services.Models.Responses;
using Bouquet.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Bouquet.Services.Aditional
{
    public class CityService : ICityService
    {
        #region Declarations

        private readonly BouquetContext _dbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public CityService(BouquetContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Взима всички градове
        /// </summary>
        /// <returns></returns>
        public async Task<Response> GetCitiesAsync()
        {
            var cities = await _dbContext.Cites.Where(c => !c.Name.Equals("default")).ToListAsync();

            if (cities == null || !cities.Any())
            {
                return new Response { Status = StatusEnum.Failure, Message = "Cities not found" };
            }

            return new Response<IEnumerable<City>> { Status = StatusEnum.Success, Data = cities };
        }

        ///// <summary>
        ///// Добавя нов град
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public async Task<Response> AddCityAsync(AddBouquetRequest request)
        //{
        //    try
        //    {
        //        var bouquet = _mapper.Map<Database.Entities.Bouquet>(request);

        //        var flowers = await _dbContext.Flowers.Where(f => bouquet.Flowers.Select(f => f.Id).Contains(f.Id)).ToListAsync();
        //        var colors = await _dbContext.Colors.Where(c => bouquet.Colors.Select(c => c.Id).Contains(c.Id)).ToListAsync();

        //        bouquet.Flowers = flowers;
        //        bouquet.Colors = colors;

        //        await _dbContext.Bouquets.AddAsync(bouquet);

        //        await _dbContext.SaveChangesAsync();

        //        return new Response { Status = StatusEnum.Success };

        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response { Status = StatusEnum.Failure, Message = ex.Message };
        //    }
        //}

        #endregion
    }
}
