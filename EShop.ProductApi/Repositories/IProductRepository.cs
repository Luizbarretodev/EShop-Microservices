using EShop.ProductApi.Models;

namespace EShop.ProductApi.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts();

    Task<Product> GetById(int id);

    Task<Product> Create(Product product);

    Task<Product> Update(Product product);

    Task<Product> Delete(int id);
}