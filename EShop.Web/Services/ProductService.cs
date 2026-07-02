using EShop.Web.Models;
using EShop.Web.Services.Contracts;
using System.Text.Json;

namespace EShop.Web.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string apiEndPoint = "/api/products/";
    private readonly JsonSerializerOptions _options;
    private ProductViewModel productVM;
    private IEnumerable<ProductViewModel> productsVM;

    public ProductService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
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
        throw new NotImplementedException();
    }

    public async Task<ProductViewModel> CreateProduct(ProductViewModel productVM)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductViewModel> UpdateProduct(ProductViewModel productVM)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteProductById(int id)
    {
        throw new NotImplementedException();
    }
}
