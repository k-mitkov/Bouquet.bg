using AutoMapper;
using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Models.DTOs;

namespace Bouquet.Services.Mapper.UserProfile
{
    public class WorkerProfile : Profile
    {
        public WorkerProfile()
        {
            CreateMap<BouquetUser, WorkerDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserInfo.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserInfo.LastName))
                .ForMember(dest => dest.UserImageUrl, opt => opt.MapFrom(src => src.ProfilePictureDataUrl))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
