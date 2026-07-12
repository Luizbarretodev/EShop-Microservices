using EShop.Web.Models;
using EShop.Web.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EShop.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _service.Login(loginVM);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(loginVM);
            }

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(user.Token);
            foreach (var claim in jwt.Claims)
            {
                Console.WriteLine($"{claim.Type} = {claim.Value}");
            }
            var role = jwt.Claims.FirstOrDefault(c =>
            c.Type == ClaimTypes.Role || c.Type == "role")?.Value ?? "Client";

            var sub = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? string.Empty;

            var claims = new List<Claim>
            {
             new Claim(ClaimTypes.Name, user.UserName),
             new Claim(ClaimTypes.Email, user.Email),
             new Claim(ClaimTypes.Role, role),
             new Claim("Token", user.Token),
            new Claim(ClaimTypes.NameIdentifier, sub)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            var user = await _service.Register(registerVM);
            if (!user)
            {
                ModelState.AddModelError(string.Empty, "Error to register user");
                return View(registerVM);
            }

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
