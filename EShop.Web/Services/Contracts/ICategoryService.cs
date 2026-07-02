using EShop.Web.Models;

namespace EShop.Web.Services.Contracts;

public interface ICategoryService
{
    Task<IEnumerable<CategoryViewmodel>> GetAllCategories();
}
