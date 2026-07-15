using EShop.Web.Models;
using EShop.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EShop.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
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

        if(cart is not null)
        {
            foreach(var item in cart.CartItems)
            {
                cart.CartHeader.TotalAmount += (item.Product.Price * item.Quantity);
            }
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


    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
