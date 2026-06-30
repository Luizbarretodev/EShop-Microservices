using EShop.ProductApi.DTOs;
using EShop.ProductApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var productsDTO = await _productService.GetProducts();

            if (productsDTO == null)
                return NotFound("Products not found");

            return Ok(productsDTO);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetById(int id)
        {
            var productDTO = await _productService.GetProductById(id);

            if (productDTO == null)
                return NotFound("Product not found");

            return Ok(productDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
                return BadRequest("Invalid data");

            await _productService.AddProduct(productDTO);

            return new CreatedAtRouteResult("GetProduct",
                                            new { id = productDTO.Id },
                                            productDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductDTO productDTO)
        {
            if (id != productDTO.CategoryId)
                return BadRequest("Invalid data");

            if (productDTO == null)
                return BadRequest("Invalid data");

            await _productService.UpdateProduct(productDTO);

            return Ok(productDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var productDTO = await _productService.GetProductById(id);

            if (productDTO == null)
                return NotFound("Category not found");

            await _productService.DeleteProduct(id);

            return Ok(productDTO);
        }
    }
}
