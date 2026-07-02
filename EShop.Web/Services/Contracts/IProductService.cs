using EShop.Web.Models;

namespace EShop.Web.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>> GetAllProducts();
    Task<ProductViewModel> FindProductById(int id);
    Task<ProductViewModel> CreateProduct();
    Task<ProductViewModel> UpdateProduct();
    Task<bool> DeleteProduct();
}
