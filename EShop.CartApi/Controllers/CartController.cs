using EShop.CartApi.DTOs;
using EShop.CartApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.CartApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;

    public CartController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    [HttpGet("applycoupon")]
    public async Task<ActionResult<CartDTO>> ApplyCoupon(CartDTO cartDTO)
    {
        var result = await _cartRepository.ApplyCouponAsync(cartDTO.CartHeader.UserId, cartDTO.CartHeader.CoupounCode);

        if (!result)
        {
            return NotFound($"CartHeader not found for user id {cartDTO.CartHeader.UserId}");
        }

        return Ok(result);
    }

    [HttpDelete("deletecoupon/{userId}")]
    public async Task<ActionResult<CartDTO>> DeleteCoupon(string UserId)
    {
        var result = await _cartRepository.DeleteCouponAsync(UserId);

        if (!result)
        {
            return NotFound($"Discount coupon not found for user id {UserId}");
        }

        return Ok(result);
    }

    [HttpGet("getcart/{id}")]
    public async Task<ActionResult<CartDTO>> GetByUserId(string id)
    {
        var cartDto = await _cartRepository.GetCartByUserIdAsync(id);

        if (cartDto == null)
        {
            return NotFound();
        }

        return Ok(cartDto);
    }

    [HttpPost("addcart")]
    public async Task<ActionResult<CartDTO>> AddCart(CartDTO cartDto)
    {
        var cart = await _cartRepository.UpdateCartAsync(cartDto);

        if (cart == null)
        {
            return NotFound();
        }

        return Ok(cart);
    }

    [HttpPut("updatecart")]
    public async Task<ActionResult<CartDTO>> UpdateCart(CartDTO cartDto)
    {
        var cart = await _cartRepository.UpdateCartAsync(cartDto);

        if (cart == null)
        {
            return NotFound();
        }

        return Ok(cart);
    }

    [HttpDelete("deletecart/{id}")]
    public async Task<ActionResult<bool>> DeleteCart(int id)
    {
        var status = await _cartRepository.DeleteCartItemAsync(id);

        if (!status)
            return BadRequest();

        return Ok(status);
    }
}
