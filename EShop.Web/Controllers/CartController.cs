using EShop.Web.Models;
using EShop.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
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
                cart.CartHeader.TotalAmout += (item.Product.Price * item.Quantity);
            }
        }

        return cart;
    }

    private string GetUserId()
    {
        return User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault().Value;
    }
}
