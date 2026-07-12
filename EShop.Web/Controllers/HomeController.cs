using EShop.CartApi.Models;
using EShop.Web.Models;
using EShop.Web.Services;
using EShop.Web.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace EShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
        {
            var result = await _productService.GetAllProducts();

            if (result is null)
                return View("Error");

            return View(result);
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ProductViewModel>> ProductDetails(int id)
        { 
            var product = await _productService.FindProductById(id);

            if (product == null)
                return View("Error");

            return View(product);
        }

        [HttpPost]
        [ActionName("ProductDetails")]  
        [Authorize]
        public async Task<ActionResult<ProductViewModel>> ProductDetailsPost(ProductViewModel productVM)
        {
            var userId = User.Claims
                .FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;

            var cart = new CartViewModel
            {
                CartHeader = new CartHeaderViewModel
                {
                    UserId = userId
                }
            };

            var cartItem = new CartItemViewModel
            {
                Quantity = productVM.Quantity,
                ProductId = productVM.Id,
                Product = await _productService.FindProductById(productVM.Id)
            };

            List<CartItemViewModel> cartItemsVm = new List<CartItemViewModel>(); 
            cartItemsVm.Add(cartItem);
            cart.CartItems = cartItemsVm;

            var result = await _cartService.AddItemToCartAsync(cart);

            if (result is not null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
