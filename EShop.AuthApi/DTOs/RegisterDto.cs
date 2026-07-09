using System.ComponentModel.DataAnnotations;

namespace EShop.AuthApi.DTOs;

public class RegisterDto
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Client"; // deixa client por padrao
}
