using AutoMapper;
using EShop.ProductApi.Models;

namespace EShop.ProductApi.DTOs.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category,CategoryDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>();
        CreateMap<Product, ProductDTO>()
            .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
    }
}

