using AutoMapper;
using Bouquet.Database.Entities;
using Bouquet.Services.Models.DTOs.Bouquet;

namespace Bouquet.Services.Mapper.Bouquet
{
    public class FlowerProfile : Profile
    {
        public FlowerProfile()
        {
            CreateMap<Flower, FlowerDTO>().ReverseMap();
        }
    }
}