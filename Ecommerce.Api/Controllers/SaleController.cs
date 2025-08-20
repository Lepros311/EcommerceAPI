﻿using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;
using Ecommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAllSales()
        {
            var responseWithDtos = await _saleService.GetAllSales();

            if (responseWithDtos.Status == ResponseStatus.Fail)
            {
                return BadRequest(responseWithDtos.Message);
            }

            return Ok(responseWithDtos.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDto>> GetSaleById(int id)
        {
            var response = await _saleService.GetSaleById(id);

            if (response.Status == ResponseStatus.Fail)
            {
                return NotFound(response.Message);
            }

            var returnedSale = response.Data;

            var saleDto = new SaleDto
            {
                SaleId = returnedSale.SaleId,
                DateAndTimeOfSale = returnedSale.DateAndTimeOfSale,
                TotalPrice = returnedSale.TotalPrice,
                LineItems = returnedSale.LineItems.Select(li => new LineItemDto
                {
                    LineItemId = li.LineItemId,
                    ProductId = li.ProductId,
                    ProductName = li.Product.ProductName,
                    Category = li.Product.Category.CategoryName,
                    Quantity = li.Quantity,
                    UnitPrice = li.UnitPrice,
                }).ToList(),
            };

            return Ok(saleDto);
        }

        [HttpPost]
        public async Task<ActionResult<SaleDto>> CreateCategory([FromBody] WriteSaleDto writeSaleDto)
        {
            var responseWithDataDto = await _saleService.CreateSale(writeSaleDto);

            if (responseWithDataDto.Message == "Something not found.")
            {
                return NotFound(responseWithDataDto.Message);
            }

            if (responseWithDataDto.Status == ResponseStatus.Fail)
            {
                return BadRequest(responseWithDataDto.Message);
            }

            return CreatedAtAction(nameof(GetSaleById),
                new { id = responseWithDataDto.Data.SaleId }, responseWithDataDto.Data);
        }

        //    [HttpPut("{id}")]
        //    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] WriteCategoryDto writeCategoryDto)
        //    {
        //        var response = await _saleService.UpdateCategory(id, writeCategoryDto);

        //        if (response.Message == "Category not found.")
        //        {
        //            return NotFound(response.Message);
        //        }

        //        if (response.Status == ResponseStatus.Fail)
        //        {
        //            return BadRequest(response.Message);
        //        }

        //        return NoContent();
        //    }

        //    [HttpDelete("{id}")]
        //    public async Task<IActionResult> DeleteCategory(int id)
        //    {
        //        var response = await _saleService.DeleteCategory(id);

        //        if (response.Message == "Category not found.")
        //        {
        //            return NotFound(response.Message);
        //        }
        //        else if (response.Message == "Cannot delete categories that contain products.")
        //        {
        //            return Conflict(response.Message);
        //        }
        //        else if (response.Status == ResponseStatus.Fail)
        //        {
        //            return BadRequest(response.Message);
        //        }

        //        return NoContent();
        //    }
    }
}
