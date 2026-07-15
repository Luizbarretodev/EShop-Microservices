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
        var cartHeader = await _context.cartHeaders
             .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeader == null)
        {
            return new CartDTO();
        }

        Cart cart = new Cart
        {
            //Obtem o header pelo userid
            CartHeader = await _context.cartHeaders.FirstOrDefaultAsync(c => c.UserId == userId),
        };

        cart.CartItems = _context.cartItems.Where(c => c.CartHeaderId == cart.CartHeader.Id)
            .Include(p => p.Product);

        return _mapper.Map<CartDTO>(cart);
    }

    public async Task<CartDTO> UpdateCartAsync(CartDTO cartDto)
    {
        Cart cart = _mapper.Map<Cart>(cartDto);

        await SaveProductInDataBase(cartDto, cart);

        var cartHeader = await _context.cartHeaders.AsNoTracking()
             .FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

        if (cartHeader is null)
        {
            CreateCartHeaderAndItems(cart);
        }
        else
        {
            await UpdateQuantityAndItems(cartDto, cart, cartHeader);
        }

        return _mapper.Map<CartDTO>(cart);
    }

    public async Task<bool> CleanCartAsync(string userId)
    {
        var cartHeader = await _context.cartHeaders.FirstOrDefaultAsync(p => p.UserId == userId);

        if (cartHeader != null)
        {
            _context.cartItems.RemoveRange(_context.cartItems.Where(c => c.CartHeaderId == cartHeader.Id));

            _context.cartHeaders.Remove(cartHeader);

            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteCartItemAsync(int cartItemId)
    {
        try
        {
            CartItem item = await _context.cartItems.FirstOrDefaultAsync(c => c.Id == cartItemId);

            int total = _context.cartItems.Where(c => c.CartHeaderId == cartItemId).Count();

            _context.cartItems.Remove(item);
            await _context.SaveChangesAsync();

            if (total == 1)
            {
                var cartHeader = await _context.cartHeaders.FirstOrDefaultAsync(c => c.Id == item.CartHeaderId);

                _context.cartHeaders.Remove(cartHeader);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    //Logica dos cupons
    public async Task<bool> ApplyCouponAsync(string userId, string couponCode)
    {
        var cartHeaderApplyCoupon = await _context.cartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeaderApplyCoupon == null)
        {
            return false;
        }

        cartHeaderApplyCoupon.CouponCode = couponCode;

        _context.cartHeaders.Update(cartHeaderApplyCoupon);

        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteCouponAsync(string userId)
    {
        var cartHeaderDeleteCoupon = await _context.cartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeaderDeleteCoupon == null)
        {
            return false;
        }

        cartHeaderDeleteCoupon.CouponCode = "";

        _context.cartHeaders.Update(cartHeaderDeleteCoupon);

        await _context.SaveChangesAsync();
        return true;
    }

    //Meotodos usados em UpdateCartAsync

    private async Task UpdateQuantityAndItems(CartDTO cartDto, Cart cart,
    CartHeader? cartHeader)
    {
        //Se CartHeader não é null
        //verifica se CartItems possui o mesmo produto
        var cartDetail = await _context.cartItems.AsNoTracking().FirstOrDefaultAsync(
            p => p.ProductId == cartDto.CartItems.FirstOrDefault()
            .ProductId && p.CartHeaderId == cartHeader.Id);

        if (cartDetail is null)
        {
            //Cria o CartItems
            cart.CartItems.FirstOrDefault().CartHeaderId = cartHeader.Id;
            cart.CartItems.FirstOrDefault().Product = null;
            _context.cartItems.Add(cart.CartItems.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
        else
        {
            //Atualiza a quantidade e o CartItems
            cart.CartItems.FirstOrDefault().Product = null;
            cart.CartItems.FirstOrDefault().Quantity += cartDetail.Quantity;
            cart.CartItems.FirstOrDefault().Id = cartDetail.Id;
            cart.CartItems.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;
            _context.cartItems.Update(cart.CartItems.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
    }


    private async Task CreateCartHeaderAndItems(Cart cart)
    {
        _context.cartHeaders.Add(cart.CartHeader);
        await _context.SaveChangesAsync();

        cart.CartItems.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
        cart.CartItems.FirstOrDefault().Product = null;

        _context.cartItems.Add(cart.CartItems.FirstOrDefault());

        await _context.SaveChangesAsync();
    }

    private async Task SaveProductInDataBase(CartDTO cartDto, Cart cart)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cartDto.CartItems.FirstOrDefault().ProductId);

        if (product == null)
        {
            _context.Products.Add(cart.CartItems.FirstOrDefault().Product);
            await _context.SaveChangesAsync();
        }
    }
}
