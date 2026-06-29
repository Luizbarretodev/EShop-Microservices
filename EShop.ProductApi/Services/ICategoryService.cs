using EShop.ProductApi.DTOs;

namespace EShop.ProductApi.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetCategories();
}