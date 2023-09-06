using AutoMapper;
using Bouquet.Database.Entities;
using Bouquet.Services.Models.DTOs.Bouquet;

namespace Bouquet.Services.Mapper.Bouquet
{
    public class PictureProfile : Profile
    {
        public PictureProfile()
        {
            CreateMap<Picture, PictureDTO>().ReverseMap();
        }
    }
}
