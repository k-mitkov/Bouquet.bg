using AutoMapper;
using Bouquet.Database.Entities;
using Bouquet.Services.Models.DTOs.FlowerShop;
using Bouquet.Services.Models.Requests;

namespace Bouquet.Services.Mapper.FlowerShop
{
    public class ShopConfigProfile : Profile
    {
        public ShopConfigProfile()
        {
            CreateMap<ShopConfig, ShopConfigDTO>().ReverseMap();
            CreateMap<ShopConfig, AddFlowerShopRequest>()
                .ForMember(dest => dest.OpenAt, opt => opt.MapFrom(src => src.OpenAt.ToString()))
                .ForMember(dest => dest.CloseAt, opt => opt.MapFrom(src => src.CloseAt.ToString()))
                .ForMember(dest => dest.SameDayTillHour, opt => opt.MapFrom(src => src.SameDayTillHour.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.OpenAt, opt => opt.MapFrom(src => TimeSpan.Parse(src.OpenAt)))
                .ForMember(dest => dest.CloseAt, opt => opt.MapFrom(src => TimeSpan.Parse(src.CloseAt)))
                .ForMember(dest => dest.SameDayTillHour, opt => opt.MapFrom(src => TimeSpan.Parse(src.SameDayTillHour)));
        }
    }
}
