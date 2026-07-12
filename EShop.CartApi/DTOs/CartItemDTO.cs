using EShop.CartApi.Models;

namespace EShop.CartApi.DTOs;

public class CartItemDTO
{
    public int Id { get; set; }
    public int Quantity { get; set; } = 1;
    public int ProductId { get; set; }
    public int CartHeaderId { get; set; }
    public Product Product { get; set; } = new Product();
}
