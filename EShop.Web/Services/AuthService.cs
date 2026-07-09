using EShop.Web.Models;
using EShop.Web.Services.Contracts;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EShop.Web.Services;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _options;
    private const string apiEndpoint = "/api/auth/";

    public AuthService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<UserViewModel> Login(LoginViewModel loginVM)
    {
        var client = _clientFactory.CreateClient("AuthApi");

        using var response = await client.PostAsJsonAsync($"{apiEndpoint}login", loginVM);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var node = JsonNode.Parse(json);

        return new UserViewModel
        {
            Token = node?["token"]?.GetValue<string>() ?? string.Empty,
            Id = node?["user"]?["id"]?.GetValue<string>() ?? string.Empty,
            Email = node?["user"]?["email"]?.GetValue<string>() ?? string.Empty,
            UserName = node?["user"]?["userName"]?.GetValue<string>() ?? string.Empty
        };
    }

    public async Task<bool> Register(RegisterViewModel registerVM)
    {
        var client = _clientFactory.CreateClient("AuthApi");
        using var response = await client.PostAsJsonAsync($"{apiEndpoint}register", registerVM);
        return response.IsSuccessStatusCode;
    }
}
