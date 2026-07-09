using System.ComponentModel.DataAnnotations;

namespace EShop.Web.Models;

public class RegisterViewModel
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
