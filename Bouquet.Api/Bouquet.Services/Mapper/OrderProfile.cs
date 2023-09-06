using AutoMapper;
using Bouquet.Services.Models.DTOs;

namespace Bouquet.Services.Mapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Database.Entities.Order, OrderDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.DeliveryDetails.Address))
                .ForMember(dest => dest.ReciverName, opt => opt.MapFrom(src => src.DeliveryDetails.ReciverName))
                .ForMember(dest => dest.ReciverPhoneNumber, opt => opt.MapFrom(src => src.DeliveryDetails.PhoneNumber))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.UserInfo.PhoneNumber))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.UserInfo.FirstName))
                .ForMember(dest => dest.PreferredTime, opt => opt.MapFrom(src => src.DeliveryDetails.PreferredTime))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.DeliveryDetails.Type))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Bouquets, opt => opt.MapFrom(src => src.OrderCart.Bouquets));
        }
    }
}
