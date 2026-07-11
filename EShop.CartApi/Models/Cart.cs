using System.Collections;

namespace EShop.CartApi.Models;

public class Cart
{
    public CartHeader CartHeader { get; set; } = new CartHeader();
    public IEnumerable<CartItem> CartItems { get; set; } = Enumerable.Empty<CartItem>();
}
