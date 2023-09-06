using AutoMapper;
using Bouquet.Services.Models.DTOs.FlowerShop;
using Bouquet.Services.Models.Requests;

namespace Bouquet.Services.Mapper.FlowerShop
{
    public class FlowerShopProfile : Profile
    {
        public FlowerShopProfile()
        {
            CreateMap<Database.Entities.FlowerShop, FlowerShopDTO>()
                .ForMember(dest => dest.Workers, opt => opt.MapFrom(src => src.Workers.Select(w => w.Id)))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name));
            CreateMap<Database.Entities.FlowerShop, AddFlowerShopRequest>().ReverseMap();
        }
    }
}
