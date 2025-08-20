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
                ProductId = li.ProductId,
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
        var response = new BaseResponse<Sale>();
        var responseWithDataDto = new BaseResponse<SaleDto>();

        var productIds = writeSaleDto.LineItems.Select(li => li.ProductId).Distinct().ToList();

        var allProducts = await _productRepository.GetAllProducts();
        var productPriceLookup = allProducts.Data.ToDictionary(p => p.ProductId, p => p.Price);
        var productLookup = allProducts.Data.ToDictionary(p => p.ProductId);
        var validProductIds = allProducts.Data.Select(p => p.ProductId).ToHashSet();

        var incomingProductIds = writeSaleDto.LineItems.Select(li => li.ProductId).ToList();
        var missingProductIds = incomingProductIds.Where(id => !validProductIds.Contains(id)).ToList();

        if (missingProductIds.Any())
        {
            responseWithDataDto.Status = ResponseStatus.Fail;
            responseWithDataDto.Message = $"Invalid Product ID(s): {string.Join(", ", missingProductIds)}";
            return responseWithDataDto;
        }

        var lineItems = writeSaleDto.LineItems.Select(li => new LineItem
        {
            ProductId = li.ProductId,
            Quantity = li.Quantity,
            UnitPrice = productPriceLookup[li.ProductId],
            Product = productLookup[li.ProductId]
        }).ToList();

        var newSale = new Sale
        {
            DateAndTimeOfSale = writeSaleDto.DateAndTimeOfSale,
            TotalPrice = lineItems.Sum(li => li.Quantity * li.UnitPrice),
            LineItems = lineItems
        };

        response = await _saleRepository.CreateSale(newSale);

        if (response.Status == ResponseStatus.Fail)
        {
            responseWithDataDto.Status = ResponseStatus.Fail;
            responseWithDataDto.Message = response.Message;
            return responseWithDataDto;
        }

        responseWithDataDto.Status = ResponseStatus.Success;

        var newSaleDto = new SaleDto
        {
            SaleId = newSale.SaleId,
            DateAndTimeOfSale = newSale.DateAndTimeOfSale,
            TotalPrice = newSale.TotalPrice,
            LineItems = newSale.LineItems.Select(li => new LineItemDto
            {
                LineItemId = li.LineItemId,
                ProductId = li.ProductId,
                ProductName = li.Product.ProductName,
                Category = li.Product.Category.CategoryName,
                Quantity = li.Quantity,
                UnitPrice = li.UnitPrice,
            }).ToList()
        };

        responseWithDataDto.Data = newSaleDto;

        return responseWithDataDto;
    }

    public async Task<BaseResponse<Sale>> UpdateSale(int id, WriteSaleDto writeSaleDto)
    {
        var response = new BaseResponse<Sale>();

        response = await GetSaleById(id);

        if (response.Status == ResponseStatus.Fail)
        {
            return response;
        }

        var existingSale = response.Data;

        var productIds = writeSaleDto.LineItems.Select(li => li.ProductId).Distinct().ToList();

        var allProducts = await _productRepository.GetAllProducts();
        var productPriceLookup = allProducts.Data.ToDictionary(p => p.ProductId, p => p.Price);
        var productLookup = allProducts.Data.ToDictionary(p => p.ProductId);
        var validProductIds = allProducts.Data.Select(p => p.ProductId).ToHashSet();

        var incomingProductIds = writeSaleDto.LineItems.Select(li => li.ProductId).ToList();
        var missingProductIds = incomingProductIds.Where(id => !validProductIds.Contains(id)).ToList();

        if (missingProductIds.Any())
        {
            response.Status = ResponseStatus.Fail;
            response.Message = $"Invalid Product ID(s): {string.Join(", ", missingProductIds)}";
            return response;
        }

        var existingLineItems = existingSale.LineItems
            .Where(li => li.LineItemId != null)
            .ToDictionary(li => li.LineItemId);

        foreach (var incomingLineItem in writeSaleDto.LineItems)
        {
            if (incomingLineItem.LineItemId.HasValue && existingLineItems.TryGetValue(incomingLineItem.LineItemId.Value, out var existingLineItem))
            {
                existingLineItem.Quantity = incomingLineItem.Quantity;
                existingLineItem.Product = productLookup[incomingLineItem.ProductId];
            }
            else
            {
                existingSale.LineItems.Add(new LineItem
                {
                    ProductId = incomingLineItem.ProductId,
                    Quantity = incomingLineItem.Quantity,
                    UnitPrice = productPriceLookup[incomingLineItem.ProductId],
                    Product = productLookup[incomingLineItem.ProductId]
                });
            }
        }

        existingSale.DateAndTimeOfSale = writeSaleDto.DateAndTimeOfSale;
        existingSale.TotalPrice = existingSale.LineItems.Sum(li => li.Quantity * li.UnitPrice);

        response = await _saleRepository.UpdateSale(existingSale);

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


