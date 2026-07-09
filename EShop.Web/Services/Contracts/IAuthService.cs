using EShop.Web.Models;

namespace EShop.Web.Services.Contracts;

public interface IAuthService
{
    Task<UserViewModel?> Login(LoginViewModel loginVM);
    Task<bool> Register(RegisterViewModel registerVM);
}
