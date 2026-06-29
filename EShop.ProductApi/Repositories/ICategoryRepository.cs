using EShop.ProductApi.Models;

namespace EShop.ProductApi.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategories();

    Task<IEnumerable<Category>> GetCategoriesProducts();

    Task<Category> GetById(int id);

    Task<Category> Create(Category category);

    Task<Category> Update(Category category);

    Task<Category> Delete(int id);
}

