using System.ComponentModel.DataAnnotations;

namespace EShop.Web.Models;

public class CartHeaderViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string CoupounCode { get; set; } = string.Empty;

    public decimal TotalAmout { get; set; } = 0.00m;
}
