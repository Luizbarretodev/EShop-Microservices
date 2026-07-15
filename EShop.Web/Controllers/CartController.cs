using EShop.CartApi.Models;
using EShop.Web.Models;
using EShop.Web.Services;
using EShop.Web.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EShop.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICouponService _couponService;
    public CartController(ICartService cartService, ICouponService couponService)
    {
        _cartService = cartService;
        _couponService = couponService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        CartViewModel? cartVM = await GetCartByUser();

        if (cartVM == null)
        {
            ModelState.AddModelError("CartNotFound", "Cart does not exists yet...");
            return View("/Views/Cart/CartNotFound.cshtml");
        }

        return View(cartVM);
    }

    private async Task<CartViewModel> GetCartByUser()
    {
        var cart = await _cartService.GetCartByUserIdAsync(GetUserId());

        if(cart?.CartHeader is not null)
        {
            if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            {
                var coupon = await _couponService.GetDiscountCoupon(cart.CartHeader.CouponCode);

                if (coupon.CouponCode != null)
                {
                    cart.CartHeader.CouponCode = coupon.CouponCode;
                }
            }
            foreach(var item in cart.CartItems)
            {
                cart.CartHeader.TotalAmount += (item.Product.Price * item.Quantity);
            }

            cart.CartHeader.TotalAmount = cart.CartHeader.TotalAmount * (cart.CartHeader.TotalAmount * cart.CartHeader.Discount) / 100;
        }

        return cart;
    }

    public async Task<IActionResult> RemoveItem(int id)
    {
        var result = await _cartService.RemoveItemFromCartAsync(id);
        if (result)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(id);
    }

    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(CartViewModel cartVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _cartService.ApplyCouponAsync(cartVM);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        return View();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCoupon()
    {
        var result = await _cartService.RemoveCouponAsync(GetUserId());

        if (result)
        {
            return RedirectToAction(nameof(Index));
        }
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        CartViewModel cartVM = await GetCartByUser();

        return View(cartVM);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CartViewModel cartVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _cartService.CheckoutAsync(cartVM.CartHeader);

            if (result != null)
            {
                return RedirectToAction(nameof(CheckoutCompleted));
            }
        }
        return View(cartVM);
    }

    [HttpGet]
    public IActionResult CheckoutCompleted()
    {
        return View();
    }


    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
