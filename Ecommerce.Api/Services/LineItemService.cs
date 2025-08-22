using Ecommerce.Api.Models;
using Ecommerce.Api.Repository;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public class LineItemService : ILineItemService
{
    private readonly ILineItemRepository _lineItemRepository;

    private readonly ISaleRepository _saleRepository;

    private readonly IProductRepository _productRepository;

    public LineItemService(ILineItemRepository lineItemRepository, ISaleRepository saleRepository, IProductRepository productRepository)
    {
        _lineItemRepository = lineItemRepository;
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<BaseResponse<List<LineItemDto>>> GetAllLineItems()
    {
        var response = new BaseResponse<List<LineItem>>();
        var responseWithDataDto = new BaseResponse<List<LineItemDto>>();

        response = await _lineItemRepository.GetAllLineItems();

        if (response.Status == ResponseStatus.Fail)
        {
            responseWithDataDto.Status = response.Status;
            responseWithDataDto.Message = response.Message;
            return responseWithDataDto;
        }

        responseWithDataDto.Data = response.Data.Select(li => new LineItemDto
        {
            LineItemId = li.LineItemId,
            ProductId = li.ProductId,
            ProductName = li.Product.ProductName,
            Category = li.Product.Category.CategoryName,
            Quantity = li.Quantity,
            UnitPrice = li.UnitPrice,
            SaleId = li.SaleId,
        }).ToList();

        return responseWithDataDto;
    }

    public async Task<BaseResponse<LineItem>> GetLineItemById(int id)
    {
        return await _lineItemRepository.GetLineItemById(id);
    }

    public async Task<BaseResponse<LineItemDto>> CreateLineItem(WriteLineItemDto writeLineItemDto)
    {
        var lineItemResponse = new BaseResponse<LineItem>();
        var lineItemResponseWithDataDto = new BaseResponse<LineItemDto>();

        var saleResponse = await _saleRepository.GetSaleById(writeLineItemDto.SaleId);

        if (saleResponse.Status == ResponseStatus.Fail)
        {
            lineItemResponseWithDataDto.Status = ResponseStatus.Fail;
            lineItemResponseWithDataDto.Message = saleResponse.Message;
            return lineItemResponseWithDataDto;
        }

        var productResponse = await _productRepository.GetProductById(writeLineItemDto.ProductId);

        if (productResponse.Status == ResponseStatus.Fail)
        {
            lineItemResponseWithDataDto.Status = ResponseStatus.Fail;
            lineItemResponseWithDataDto.Message = saleResponse.Message;
            return lineItemResponseWithDataDto;
        }

        if (writeLineItemDto.Quantity <= 0)
        {
            lineItemResponseWithDataDto.Status = ResponseStatus.Fail;
            lineItemResponseWithDataDto.Message = "Quantity must be greater than 0.";
            return lineItemResponseWithDataDto;
        }

        var newLineItem = new LineItem
        {
            ProductId = writeLineItemDto.ProductId,
            Quantity = writeLineItemDto.Quantity,
            SaleId = writeLineItemDto.SaleId,
        };

        newLineItem.Product = productResponse.Data;
        newLineItem.Sale = saleResponse.Data;

        lineItemResponse = await _lineItemRepository.CreateLineItem(newLineItem);

        if (lineItemResponse.Status == ResponseStatus.Fail)
        {
            lineItemResponseWithDataDto.Status = ResponseStatus.Fail;
            lineItemResponseWithDataDto.Message = lineItemResponse.Message;
            return lineItemResponseWithDataDto;
        }
        else
        {
            lineItemResponseWithDataDto.Status = ResponseStatus.Success;

            var newlineItemDto = new LineItemDto
            {
                LineItemId = newLineItem.LineItemId,
                ProductId = newLineItem.ProductId,
                Quantity = newLineItem.Quantity,
                SaleId = newLineItem.SaleId,
            };

            lineItemResponseWithDataDto.Data = newlineItemDto;
        }

        return lineItemResponseWithDataDto;
    }

    public async Task<BaseResponse<LineItem>> UpdateLineItem(int id, WriteLineItemDto writeLineItemDto)
    {
            var lineItemResponse = new BaseResponse<LineItem>();

        //    productResponse = await GetProductById(id);

        //    if (productResponse.Status == ResponseStatus.Fail)
        //    {
        //        return productResponse;
        //    }

        //    if (writeProductDto.Price < 0)
        //    {
        //        productResponse.Status = ResponseStatus.Fail;
        //        productResponse.Message = "Product price cannot be less than 0.";
        //        return productResponse;
        //    }

        //    var existingProduct = productResponse.Data;

        //    existingProduct.ProductName = writeProductDto.ProductName;
        //    existingProduct.Price = writeProductDto.Price;

        //    var categoryResponse = await _categoryRepository.GetCategoryById(writeProductDto.CategoryId);

        //    if (categoryResponse.Status == ResponseStatus.Fail)
        //    {
        //        productResponse.Status = ResponseStatus.Fail;
        //        productResponse.Message = categoryResponse.Message;
        //        return productResponse;
        //    }

        //    existingProduct.CategoryId = writeProductDto.CategoryId;

        //    productResponse = await _productRepository.UpdateProduct(existingProduct);

        return lineItemResponse;
    }

    public async Task<BaseResponse<LineItem>> DeleteLineItem(int id)
    {
        //    var response = new BaseResponse<Product>();

        //    response = await _productRepository.GetProductById(id);

        //    if (response.Status == ResponseStatus.Fail)
        //    {
        //        return response;
        //    }

        return await _lineItemRepository.DeleteLineItem(id);
    }
}
