using AutoMapper;
using Bouquet.Database;
using Bouquet.Services.Interfaces.Aditional;
using Bouquet.Services.Models.DTOs.Bouquet;
using Bouquet.Services.Models.Responses;
using Bouquet.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Bouquet.Services.Aditional
{
    public class ColorService : IColorService
    {
        #region Declarations

        private readonly BouquetContext _dbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public ColorService(BouquetContext dbContext, IMapper mapper)
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
        public async Task<Response> GetColorsAsync()
        {
            var colors = await _dbContext.Colors.ToListAsync();

            if (colors == null || !colors.Any())
            {
                return new Response { Status = StatusEnum.Failure, Message = "Colors not found" };
            }

            return new Response<IEnumerable<ColorDTO>> { Status = StatusEnum.Success, Data = _mapper.Map<IEnumerable<ColorDTO>>(colors) };
        }

        ///// <summary>
        ///// Добавя нов букет
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public async Task<Response> AddBouquetsAsync(AddBouquetRequest request)
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
