using EShop.Web.Models;
using EShop.Web.Services.Contracts;
using System.Text.Json;

namespace EShop.Web.Services;

public class CouponService : ICouponService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JsonSerializerOptions _options;
    private const string apiEndpoint = "/api/coupon/";
    private CouponViewModel couponVM = new CouponViewModel();

    public CouponService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<CouponViewModel> GetDiscountCoupon(string couponCode)
    {
        var client = _clientFactory.CreateClient("DiscountApi");
        AddAuthorizationHeader(client);

        using (var response = await client.GetAsync($"{apiEndpoint}{couponCode}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                couponVM = await JsonSerializer.DeserializeAsync<CouponViewModel>
                    (apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return couponVM;
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
