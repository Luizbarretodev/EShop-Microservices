using AutoMapper;
using EShop.ProductApi.DTOs;
using EShop.ProductApi.Repositories;

namespace EShop.ProductApi.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public Task<IEnumerable<CategoryDTO>> GetCategories()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CategoryDTO>> GetCategoriesProducts()
    {
        throw new NotImplementedException();
    }

    public Task<CategoryDTO> GetCategoryById(int id)
    {
        throw new NotImplementedException();
    }


    public Task AddCategory(CategoryDTO categoryDTO)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCategory(CategoryDTO categoryDTO)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCategory(int id)
    {
        throw new NotImplementedException();
    }
}
