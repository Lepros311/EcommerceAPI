using Ecommerce.Api.Models;
using Ecommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
        {
            BaseResponse<List<Product>>? response = null;

            try
            {
                response = await _productService.GetAllProducts();

                if (response.Status == ResponseStatus.Fail)
                {
                    return BadRequest(response.Message);
                }

                var returnedProducts = response.Data;

                var productDtos = returnedProducts.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Category = p.Category.CategoryName
                }).ToList();

                return Ok(productDtos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                var message = response?.Message ?? $"Internal Server Error: {ex.Message}";
                return StatusCode(500, message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            BaseResponse<Product>? response = null;

            try
            {
                response = await _productService.GetProductById(id);

                if (response.Status == ResponseStatus.Fail)
                {
                    return BadRequest(response.Message);
                }

                var returnedProduct = response.Data;

                var productDto = new ProductDto
                {
                    ProductId = returnedProduct.ProductId,
                    ProductName = returnedProduct.ProductName,
                    Price = returnedProduct.Price,
                    Category = returnedProduct.Category.CategoryName
                };

                return Ok(productDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                var message = response?.Message ?? $"Internal Server Error: {ex.Message}";
                return StatusCode(500, message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto dto)
        {
            BaseResponse<Product>? response = null;

            try
            {
                var product = new Product
                {
                    ProductName = dto.ProductName,
                    Price = dto.Price,
                    CategoryId = dto.CategoryId
                };

                response = await _productService.CreateProduct(product);

                if (response.Status == ResponseStatus.Fail)
                {
                    return BadRequest(response.Message);
                }

                var createdProduct = response.Data;

                var productDto = new ProductDto
                {
                    ProductId = createdProduct.ProductId,
                    ProductName = createdProduct.ProductName,
                    Price = createdProduct.Price,
                    Category = createdProduct.Category?.CategoryName
                };

                return CreatedAtAction(nameof(GetProductById), new { id = productDto.ProductId }, productDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                var message = response?.Message ?? $"Internal Server Error: {ex.Message}";
                return StatusCode(500, message);
            }
        }
    }
}
