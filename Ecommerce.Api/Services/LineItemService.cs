using Ecommerce.Api.Models;
using Ecommerce.Api.Repository;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public class LineItemService : ILineItemService
{
    private readonly ILineItemRepository _lineItemRepository;

    private readonly ICategoryRepository _categoryRepository;

    public LineItemService(ILineItemRepository lineItemRepository, ICategoryRepository categoryRepository)
    {
        _lineItemRepository = lineItemRepository;
        _categoryRepository = categoryRepository;
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

        responseWithDataDto.Data = response.Data.Select(p => new LineItemDto
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Price = p.Price,
            Category = p.Category.CategoryName
        }).ToList();

        return responseWithDataDto;
    }

    public async Task<BaseResponse<LineItem>> GetLineItemById(int id)
    {
        return await _lineItemRepository.GetLineItemById(id);
    }

    public async Task<BaseResponse<LineItemDto>> CreateLineItem(WriteLineItemDto writeLineItemDto)
    {
        //    var productResponse = new BaseResponse<Product>();
            var productResponseWithDataDto = new BaseResponse<LineItemDto>();

        //    var categoryResponse = await _categoryRepository.GetCategoryById(writeProductDto.CategoryId);

        //    if (categoryResponse.Status == ResponseStatus.Fail)
        //    {
        //        productResponseWithDataDto.Status = ResponseStatus.Fail;
        //        productResponseWithDataDto.Message = categoryResponse.Message;
        //        return productResponseWithDataDto;
        //    }

        //    if (writeProductDto.Price < 0)
        //    {
        //        productResponseWithDataDto.Status = ResponseStatus.Fail;
        //        productResponseWithDataDto.Message = "Product price cannot be less than 0.";
        //        return productResponseWithDataDto;
        //    }

        //    var newProduct = new Product
        //    {
        //        ProductName = writeProductDto.ProductName,
        //        Price = writeProductDto.Price,
        //        CategoryId = writeProductDto.CategoryId
        //    };

        //    newProduct.Category = categoryResponse.Data;

        //    productResponse = await _productRepository.CreateProduct(newProduct);

        //    if (productResponse.Status == ResponseStatus.Fail)
        //    {
        //        productResponseWithDataDto.Status = ResponseStatus.Fail;
        //        productResponseWithDataDto.Message = productResponse.Message;
        //        return productResponseWithDataDto;
        //    }
        //    else
        //    {
        //        productResponseWithDataDto.Status = ResponseStatus.Success;

        //        var newProductDto = new ProductDto
        //        {
        //            ProductId = newProduct.ProductId,
        //            ProductName = newProduct.ProductName,
        //            Price = newProduct.Price,
        //            Category = newProduct.Category?.CategoryName
        //        };

        //        productResponseWithDataDto.Data = newProductDto;
        //    }

        return productResponseWithDataDto;
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
