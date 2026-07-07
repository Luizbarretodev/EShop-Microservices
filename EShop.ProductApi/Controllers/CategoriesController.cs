using EShop.ProductApi.DTOs;
using EShop.ProductApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categoriesDTO = await _categoryService.GetCategories();

            if (categoriesDTO == null)
                return NotFound("Categories not found");

            return Ok(categoriesDTO);
        }

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetProducts()
        {
            var categoriesDTO = await _categoryService.GetCategoriesProducts();

            if (categoriesDTO == null)
                return NotFound("Categories not found");

            return Ok(categoriesDTO);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetById(int id)
        {
            var categoryDTO = await _categoryService.GetCategoryById(id);

            if (categoryDTO == null)
                return NotFound("Category not found");

            return Ok(categoryDTO);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
                return BadRequest("Invalid data");

            await _categoryService.AddCategory(categoryDTO);

            return new CreatedAtRouteResult("GetCategory",
                                            new { id = categoryDTO.CategoryId },
                                            categoryDTO);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.CategoryId)
                return BadRequest("Invalid data");

            if (categoryDTO == null)
                return BadRequest("Invalid data");

            await _categoryService.UpdateCategory(categoryDTO);

            return Ok(categoryDTO);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryDTO = await _categoryService.GetCategoryById(id);

            if (categoryDTO == null)
                return NotFound("Category not found");

            await _categoryService.DeleteCategory(id);

            return Ok(categoryDTO);
        }
    }
}
