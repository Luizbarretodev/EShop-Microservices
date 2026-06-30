using AutoMapper;
using EShop.ProductApi.DTOs;
using EShop.ProductApi.Models;
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

    public async Task<IEnumerable<ProductDTO>> GetProducts()
    {
        var productsEntity = await _productRepository.GetAll();
        return _mapper.Map<IEnumerable<ProductDTO>>(productsEntity);
    }

    public async Task<ProductDTO> GetProductById(int id)
    {
        var productEntity = await _productRepository.GetById(id);
        return _mapper.Map<ProductDTO>(productEntity);
    }

    public async Task AddProduct(ProductDTO productDTO)
    {
        var productEntity = _mapper.Map<Product>(productDTO);
        await _productRepository.Create(productEntity);

        productDTO.CategoryId = productEntity.CategoryId;
    }
    public async Task UpdateProduct(ProductDTO productDTO)
    {
        var productEntity = _mapper.Map<Product>(productDTO);
        await _productRepository.Create(productEntity);
    }

    public async Task DeleteProduct(int id)
    {
        var productEntity = GetProductById(id).Result;
        await _productRepository.Delete(productEntity.Id);
    }
}
