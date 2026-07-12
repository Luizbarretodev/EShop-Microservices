using EShop.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;

namespace EShop.Web.Services;

public class CartService : ICartService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private JsonSerializerOptions? _options;
    private const string apiEndpoint = "/api/cart";
    private CartViewModel cartVM = new CartViewModel();

    public CartService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _clientFactory = clientFactory;
        _httpContextAccessor = httpContextAccessor;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<CartViewModel?> GetCartByUserIdAsync(string userId)
    {
        var client = _clientFactory.CreateClient("CartApi");
        AddAuthorizationHeader(client); 

        using var response = await client.GetAsync($"{apiEndpoint}/getcart/{userId}");
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartVM = await JsonSerializer.DeserializeAsync<CartViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
            return cartVM;
        }
    }

    public async Task<CartViewModel> AddItemToCartAsync(CartViewModel cartVM)
    {
        var client = _clientFactory.CreateClient("CartApi");
        AddAuthorizationHeader(client);

        StringContent content = new StringContent(JsonSerializer.Serialize(cartVM), 
                                                  Encoding.UTF8, "application/json");

        using var response = await client.PostAsync($"{apiEndpoint}/addcart/", content);
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartVM = await JsonSerializer.DeserializeAsync<CartViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
            return cartVM;
        }
    }   

    public async Task<CartViewModel> UpdateCartAsync(CartViewModel cartVM)
    {
        var client = _clientFactory.CreateClient("CartApi");
        AddAuthorizationHeader(client);

        CartViewModel cartUpdated = new CartViewModel();    

        using var response = await client.PutAsJsonAsync($"{apiEndpoint}/updatecart/", cartVM);
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartUpdated = await JsonSerializer.DeserializeAsync<CartViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
            return cartUpdated;
        }
    }

    public async Task<bool> RemoveItemFromCartAsync(int cartId)
    {
        var client = _clientFactory.CreateClient("CartApi");
        AddAuthorizationHeader(client);

        using var response = await client.DeleteAsync($"{apiEndpoint}/deletecart/" + cartId);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> ApplyCouponAsync(CartViewModel cartVM, string coupon)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveCouponAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CleanCartAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<CartViewModel> CheckoutAsync(CartViewModel cartVM)
    {
        throw new NotImplementedException();
    }


    private void AddAuthorizationHeader(HttpClient client)
    {
        var token = _httpContextAccessor.HttpContext?.User
            .FindFirst("Token")?.Value;

        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

}
