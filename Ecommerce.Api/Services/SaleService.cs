using Ecommerce.Api.Models;
using Ecommerce.Api.Repository;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public class SaleService : ISaleService
{
    private readonly IProductRepository _productRepository;

    private readonly ISaleRepository _saleRepository;

    public SaleService(IProductRepository productRepository, ISaleRepository saleRepository)
    {
        _productRepository = productRepository;
        _saleRepository = saleRepository;
    }

    public async Task<BaseResponse<List<SaleDto>>> GetAllSales()
    {
        var response = new BaseResponse<List<Sale>>();
        var responseWithDataDto = new BaseResponse<List<SaleDto>>();

        response = await _saleRepository.GetAllSales();

        if (response.Status == ResponseStatus.Fail)
        {
            responseWithDataDto.Status = response.Status;
            responseWithDataDto.Message = response.Message;
            return responseWithDataDto;
        }

        responseWithDataDto.Data = response.Data.Select(s => new SaleDto
        {
            SaleId = s.SaleId,
            DateAndTimeOfSale = s.DateAndTimeOfSale,
            TotalPrice = s.TotalPrice,
            LineItems = s.LineItems.Select(li => new LineItemDto
            {
                LineItemId = li.LineItemId,
                ProductName = li.Product.ProductName,
                Category = li.Product.Category.CategoryName,
                Quantity = li.Quantity,
                UnitPrice = li.UnitPrice,
            }).ToList()
        }).ToList();

        return responseWithDataDto;
    }

    public async Task<BaseResponse<Sale>> GetSaleById(int id)
    {
        return await _saleRepository.GetSaleById(id);
    }

    public async Task<BaseResponse<SaleDto>> CreateSale(WriteSaleDto writeSaleDto)
    {
        //        var response = new BaseResponse<Category>();
                var responseWithDataDto = new BaseResponse<SaleDto>();

        //        var newCategory = new Category
        //        {
        //            CategoryName = writeCategoryDto.CategoryName
        //        };

        //        response = await _categoryRepository.CreateCategory(newCategory);

        //        if (response.Status == ResponseStatus.Fail)
        //        {
        //            responseWithDataDto.Status = ResponseStatus.Fail;
        //            responseWithDataDto.Message = response.Message;
        //            return responseWithDataDto;
        //        }
        //        else
        //        {
        //            responseWithDataDto.Status = ResponseStatus.Success;

        //            var newCategoryDto = new CategoryDto
        //            {
        //                CategoryId = newCategory.CategoryId,
        //                CategoryName = newCategory.CategoryName,
        //            };

        //            responseWithDataDto.Data = newCategoryDto;
        //        }

        return responseWithDataDto;
    }

    public async Task<BaseResponse<Sale>> UpdateSale(int id, WriteSaleDto writeSaleDto)
    {
                var response = new BaseResponse<Sale>();

        //        response = await GetCategoryById(id);

        //        if (response.Status == ResponseStatus.Fail)
        //        {
        //            return response;
        //        }

        //        var existingCategory = response.Data;

        //        existingCategory.CategoryName = writeCategoryDto.CategoryName;

        //        response = await _categoryRepository.UpdateCategory(existingCategory);

        return response;
    }

    public async Task<BaseResponse<Sale>> DeleteSale(int id)
    {
        //var response = new BaseResponse<Sale>();

        //        response = await _categoryRepository.GetCategoryById(id);

        //        if (response.Status == ResponseStatus.Fail)
        //        {
        //            return response;
        //        }

        //        if (response.Data.Products.Count > 0)
        //        {
        //            response.Message = "Cannot delete categories that contain products.";
        //            return response;
        //        }

        return await _saleRepository.DeleteSale(id);
    }
}


