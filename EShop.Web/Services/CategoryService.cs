using EShop.Web.Models;
using EShop.Web.Services.Contracts;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EShop.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        private const string apiEndPoint = "/api/categories/";
        private readonly JsonSerializerOptions _options;

        public CategoryService(IHttpClientFactory clientFactory, IHttpContextAccessor contextAccessor)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _contextAccessor = contextAccessor;
        }
        private void AddAuthorization(HttpClient client)
        {
            var token = _contextAccessor.HttpContext?.User
                .FindFirst("Token")?.Value;

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IEnumerable<CategoryViewmodel>> GetAllCategories()
        {
            var client = _clientFactory.CreateClient("ProductApi");
            AddAuthorization(client);

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
