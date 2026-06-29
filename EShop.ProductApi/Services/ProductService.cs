using AutoMapper;
using EShop.ProductApi.DTOs;
using EShop.ProductApi.Repositories;

namespace EShop.ProductApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public Task<IEnumerable<ProductDTO>> GetProducts()
    {
        throw new NotImplementedException();
    }

    public Task<ProductDTO> GetProductById(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddProduct(ProductDTO productDTO)
    {
        throw new NotImplementedException();
    }
    public Task UpdateProduct(ProductDTO productDTO)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }
}
