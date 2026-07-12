using EShop.Web.Models;

namespace EShop.Web.Services;

public interface ICartService
{
    Task<CartViewModel> GetCartByUserIdAsync(string userid);
    Task<CartViewModel> AddItemToCartAsync(CartViewModel cartVM);
    Task<CartViewModel> UpdateCartAsync(CartViewModel cartVM);
    Task<bool> RemoveItemFromCartAsync(int id);

    Task<bool> ApplyCouponAsync(CartViewModel cartVM, string coupon);
    Task<bool> RemoveCouponAsync(int id);
    Task<bool> CleanCartAsync(int id);

    Task<CartViewModel> CheckoutAsync(CartViewModel cartVM);
}
