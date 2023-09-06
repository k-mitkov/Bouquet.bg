using AutoMapper;
using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Models.DTOs;

namespace Bouquet.Services.Mapper
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<UserInfo, UserInfoDTO>().ReverseMap();
        }
    }
}
