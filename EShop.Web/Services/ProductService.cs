using EShop.Web.Models;
using EShop.Web.Services.Contracts;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EShop.Web.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _contextAccessor;
    private const string apiEndPoint = "/api/products/";
    private readonly JsonSerializerOptions _options;
    private ProductViewModel productVM;
    private IEnumerable<ProductViewModel> productsVM;

    public ProductService(IHttpClientFactory clientFactory, IHttpContextAccessor contextAccessor)
    {
        _clientFactory = clientFactory;
        _contextAccessor = contextAccessor;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public void AddAuthorization(HttpClient client)
    {
        var token = _contextAccessor.HttpContext.User.FindFirst("Token").Value;

        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<IEnumerable<ProductViewModel>> GetAllProducts()
    {
        var client = _clientFactory.CreateClient("ProductApi");

        using (var response = await client.GetAsync(apiEndPoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                productsVM = await JsonSerializer.DeserializeAsync<IEnumerable<ProductViewModel>>(apiResponse, _options);
            }
            else
            {
                return null;
            }

            return productsVM;
        }
    }

    public async Task<ProductViewModel> FindProductById(int id)
    {
        var client = _clientFactory.CreateClient("ProductApi");

        using (var response = await client.GetAsync(apiEndPoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                productVM = await JsonSerializer.DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }

            return productVM;
        }
    }

    public async Task<ProductViewModel> CreateProduct(ProductViewModel productVM)
    {
        var content = new StringContent(
        JsonSerializer.Serialize(productVM),
        Encoding.UTF8,
        "application/json");

        var client = _clientFactory.CreateClient("ProductApi");
        AddAuthorization(client);

        using (var response = await client.PostAsync(apiEndPoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                productVM = await JsonSerializer.DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }

            return productVM;
        }
    }

    public async Task<ProductViewModel> UpdateProduct(ProductViewModel productVM)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        AddAuthorization(client);
        ProductViewModel productUpdated = new ProductViewModel();

        using (var response = await client.PutAsJsonAsync(apiEndPoint, productVM))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                productUpdated = await JsonSerializer.DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }

            return productUpdated;
        }
    }

    public async Task<bool> DeleteProductById(int id)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        AddAuthorization(client);

        using (var response = await client.DeleteAsync(apiEndPoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
               return true;
            }
            else
            {
                return false;
            }
        }
    }
}
