using Microsoft.AspNetCore.Identity;

namespace EShop.AuthApi.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(IdentityUser applicationUser);
}
