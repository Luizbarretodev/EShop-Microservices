using Microsoft.AspNetCore.Identity;

namespace EShop.IdentityServer.Data;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = String.Empty;
    public string lastName { get; set; } = String.Empty;
}