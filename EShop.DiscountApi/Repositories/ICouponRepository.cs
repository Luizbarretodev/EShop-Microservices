using EShop.DiscountApi.DTOs;

namespace EShop.DiscountApi.Repositories;

public interface ICouponRepository
{
    Task<CouponDTO> GetCouponById(string couponCode);
}
