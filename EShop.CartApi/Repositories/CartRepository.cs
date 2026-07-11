using AutoMapper;
using EShop.CartApi.Context;
using EShop.CartApi.DTOs;
using EShop.CartApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.CartApi.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CartRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CartDTO> GetCartByUserIdAsync(string userId)
    {
        Cart cart = new Cart
        {
            //Obtem o header pelo userid
            CartHeader = await _context.cartHeaders.FirstOrDefaultAsync(c => c.UserId == userId),
        };

        cart.CartItems = _context.cartItems.Where(c => c.CartHeaderId == cart.CartHeader.Id)
            .Include(p => p.Product);

        return _mapper.Map<CartDTO>(cart);
    }

    public async Task<CartDTO> UpdateCartAsync(CartDTO cart)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CleanCartAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteCartItemAsync(int cartItemId)
    {
        throw new NotImplementedException();
    }


    public async Task<bool> ApplyCouponAsync(string userId, string couponCode)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> DeleteCouponAsync(string userId)
    {
        throw new NotImplementedException();
    }
}
