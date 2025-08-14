using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;
using Ecommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var response = await _productService.GetAllProducts();

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

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var response = await _productService.GetProductById(id);

            if (response.Status == ResponseStatus.Fail)
            {
                return NotFound(response.Message);
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

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] WriteProductDto writeProductDto)
        {
            var product = new Product
            {
                ProductName = writeProductDto.ProductName,
                Price = writeProductDto.Price,
                CategoryId = writeProductDto.CategoryId
            };

            var response = await _productService.CreateProduct(product);

            if (response.Message == "Category not found.")
            {
                return NotFound(response.Message);
            }

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

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] WriteProductDto writeProductDto)
        {
            var response = await _productService.GetProductById(id);

            if (response.Status == ResponseStatus.Fail)
            {
                return NotFound(response.Message);
            }

            var existingProduct = response.Data;

            existingProduct.ProductName = writeProductDto.ProductName;
            existingProduct.Price = writeProductDto.Price;
            existingProduct.CategoryId = writeProductDto.CategoryId;

            response = await _productService.UpdateProduct(existingProduct);

            if (response.Status == ResponseStatus.Fail)
            {
                return BadRequest(response.Message);
            }

            var updatedProduct = response.Data;

            var productDto = new ProductDto
            {
                ProductId = updatedProduct.ProductId,
                ProductName = updatedProduct.ProductName,
                Price = updatedProduct.Price,
                Category = updatedProduct.Category?.CategoryName
            };

            return Ok(productDto);
        }
    }
}
