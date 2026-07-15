using AutoMapper;
using EShop.DiscountApi.Models;

namespace EShop.DiscountApi.DTOs.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CouponDTO, Coupon>().ReverseMap();
    }
}
