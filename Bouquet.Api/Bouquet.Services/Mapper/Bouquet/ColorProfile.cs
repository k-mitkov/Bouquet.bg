using AutoMapper;
using Bouquet.Database.Entities;
using Bouquet.Services.Models.DTOs.Bouquet;

namespace Bouquet.Services.Mapper.Bouquet
{
    public class ColorProfile : Profile
    {
        public ColorProfile()
        {
            CreateMap<Color, ColorDTO>().ReverseMap();
        }
    }
}