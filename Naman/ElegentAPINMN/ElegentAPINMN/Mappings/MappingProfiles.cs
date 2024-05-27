using AutoMapper;
using ElegentAPINMN.Models.Domain;
using ElegentAPINMN.Models.DTO;

namespace ElegentAPINMN.Mappings
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Discount, DiscountDto>().ReverseMap();
        }
    }
}
