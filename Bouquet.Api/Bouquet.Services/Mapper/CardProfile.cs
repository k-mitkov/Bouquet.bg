using AutoMapper;
using Bouquet.Database.Entities.Payments;
using Bouquet.Services.Models.DTOs;

namespace Bouquet.Services.Mapper
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<Card, CardDTO>().ReverseMap();
        }
    }
}
