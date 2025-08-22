using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;
using Ecommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/lineitems")]
    [ApiController]
    public class LineItemController : ControllerBase
    {
        private readonly ILineItemService _lineItemService;

        public LineItemController(ILineItemService lineItemService)
        {
            _lineItemService = lineItemService;
        }

        [HttpGet]
        public async Task<ActionResult<List<LineItemDto>>> GetAllLineItems()
        {
            var responseWithDtos = await _lineItemService.GetAllLineItems();

            if (responseWithDtos.Status == ResponseStatus.Fail)
            {
                return BadRequest(responseWithDtos.Message);
            }

            return Ok(responseWithDtos.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LineItemDto>> GetLineItemById(int id)
        {
            var response = await _lineItemService.GetLineItemById(id);

            if (response.Status == ResponseStatus.Fail)
            {
                return NotFound(response.Message);
            }

            var returnedLineItem = response.Data;

            var lineItemDto = new LineItemDto
            {
                LineItemId = returnedLineItem.LineItemId,
                ProductId = returnedLineItem.ProductId,
                ProductName = returnedLineItem.Product.ProductName,
                Category = returnedLineItem.Product.Category.CategoryName,
                Quantity = returnedLineItem.Quantity,
                UnitPrice = returnedLineItem.UnitPrice,
                SaleId = returnedLineItem.SaleId,
            };

            return Ok(lineItemDto);
        }

        //        [HttpPost]
        //        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] WriteProductDto writeProductDto)
        //        {
        //            var responseWithDataDto = await _productService.CreateProduct(writeProductDto);

        //            if (responseWithDataDto.Message == "Category not found.")
        //            {
        //                return NotFound(responseWithDataDto.Message);
        //            }

        //            if (responseWithDataDto.Status == ResponseStatus.Fail)
        //            {
        //                return BadRequest(responseWithDataDto.Message);
        //            }

        //            return CreatedAtAction(nameof(GetProductById),
        //                new { id = responseWithDataDto.Data.ProductId }, responseWithDataDto.Data);
        //        }

        //        [HttpPut("{id}")]
        //        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] WriteProductDto writeProductDto)
        //        {
        //            var response = await _productService.UpdateProduct(id, writeProductDto);

        //            if (response.Message == "Product not found." || response.Message == "Category not found.")
        //            {
        //                return NotFound(response.Message);
        //            }

        //            if (response.Status == ResponseStatus.Fail)
        //            {
        //                return BadRequest(response.Message);
        //            }

        //            return NoContent();
        //        }

        //        [HttpDelete("{id}")]
        //        public async Task<IActionResult> DeleteProduct(int id)
        //        {
        //            var response = await _productService.DeleteProduct(id);

        //            if (response.Message == "Product not found.")
        //            {
        //                return NotFound(response.Message);
        //            }
        //            else if (response.Status == ResponseStatus.Fail)
        //            {
        //                return BadRequest(response.Message);
        //            }

        //            return NoContent();
        //        }
    }
}
