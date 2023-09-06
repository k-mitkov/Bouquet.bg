using AutoMapper;
using Bouquet.Services.Models.DTOs.Bouquet;
using Bouquet.Services.Models.Requests;

namespace Bouquet.Services.Mapper.Bouquet
{
    public class BouquetProfile : Profile
    {
        public BouquetProfile()
        {
            CreateMap<Database.Entities.Bouquet, BouquetDTO>().ReverseMap();
            CreateMap<Database.Entities.CartBouquet, CartBouquetDTO>().ReverseMap();
            CreateMap<Database.Entities.Bouquet, AddBouquetRequest>().ReverseMap();
        }
    }
}
