using EShop.Web.Models;
using EShop.Web.Services.Contracts;
using System.Text.Json;

namespace EShop.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string apiEndPoint = "/api/categories/";
        private readonly JsonSerializerOptions _options;

        public CategoryService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        }

        public async Task<IEnumerable<CategoryViewmodel>> GetAllCategories()
        {
            var client = _clientFactory.CreateClient("ProductApi");

            IEnumerable<CategoryViewmodel> categories;

            var response = await client.GetAsync(apiEndPoint);

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                categories = await JsonSerializer.DeserializeAsync<IEnumerable<CategoryViewmodel>>(apiResponse, _options);
            }
            else
            {
                return null;
            }

            return categories;
        }
    }
}
