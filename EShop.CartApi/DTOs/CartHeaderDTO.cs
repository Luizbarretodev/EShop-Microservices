using System.ComponentModel.DataAnnotations;

namespace EShop.CartApi.DTOs;

public class CartHeaderDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "User Id is required")]
    public string UserId { get; set; } = string.Empty;
    public string CoupounCode { get; set; } = string.Empty;
}
