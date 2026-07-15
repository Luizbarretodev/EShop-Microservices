using EShop.Web.Models;

namespace EShop.Web.Services.Contracts;

public interface ICouponService
{
    Task<CouponViewModel> GetDiscountCoupon(string couponCode);
}
