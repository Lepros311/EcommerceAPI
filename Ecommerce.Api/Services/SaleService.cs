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

        if (writeSaleDto.LineItems.Select(li => li.Quantity < 0).Any())
        {
            responseWithDataDto.Status = ResponseStatus.Fail;
            responseWithDataDto.Message = "Quantity must be greater than 0.";
            return responseWithDataDto;
        }

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

    public async Task<BaseResponse<Sale>> UpdateSale(int id, UpdateSaleDto updateSaleDto)
    {
        var response = new BaseResponse<Sale>();

        response = await GetSaleById(id);

        if (response.Status == ResponseStatus.Fail)
        {
            return response;
        }

        if (updateSaleDto.LineItems.Select(li => li.Quantity < 0).Any())
        {
            response.Status = ResponseStatus.Fail;
            response.Message = "Quantity must be greater than 0.";
            return response;
        }

        var existingSale = response.Data;

        var existingLineItems = existingSale.LineItems.Where(li => li.LineItemId != null).ToDictionary(li => li.LineItemId);
        var validLineItemIds = existingSale.LineItems.Where(li => li.LineItemId != 0).Select(li => li.LineItemId).ToHashSet();
        var incomingLineItemIds = updateSaleDto.LineItems.Where(li => li.LineItemId.HasValue && li.LineItemId.Value != 0).Select(li => li.LineItemId.Value).ToList();
        var invalidLineItemIds = incomingLineItemIds.Where(id => !validLineItemIds.Contains(id)).ToList();

        if (invalidLineItemIds.Any())
        {
            response.Status = ResponseStatus.Fail;
            response.Message = $"Invalid Line Item ID(s): {string.Join(", ", invalidLineItemIds)}";
            return response;
        }

        var productIds = updateSaleDto.LineItems.Select(li => li.ProductId).Distinct().ToList();

        var allProducts = await _productRepository.GetAllProducts();
        var productPriceLookup = allProducts.Data.ToDictionary(p => p.ProductId, p => p.Price);
        var productLookup = allProducts.Data.ToDictionary(p => p.ProductId);
        var validProductIds = allProducts.Data.Select(p => p.ProductId).ToHashSet();

        var incomingProductIds = updateSaleDto.LineItems.Select(li => li.ProductId).ToList();
        var missingProductIds = incomingProductIds.Where(id => !validProductIds.Contains(id)).ToList();

        var existingProductIds = existingSale.LineItems.Select(li => li.ProductId).ToHashSet();
        var incomingNewProductIds = updateSaleDto.LineItems.Where(li => !li.LineItemId.HasValue).Select(li => li.ProductId).ToList();
        var duplicateProductIds = incomingNewProductIds.Where(pid => existingProductIds.Contains(pid)).ToList();
        var newLineItems = updateSaleDto.LineItems.Where(li => !existingSale.LineItems.Any(e => e.LineItemId == li.LineItemId)).ToList();
        var conflictingProductIds = newLineItems.Select(li => li.ProductId).Where(pid => existingProductIds.Contains(pid)).Distinct().ToList();
        var duplicateProductIdsInNewItems = newLineItems.GroupBy(li => li.ProductId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        var allConflictingProductIds = duplicateProductIdsInNewItems.Concat(conflictingProductIds).Distinct().ToList();

        if (allConflictingProductIds.Any())
        {
            response.Status = ResponseStatus.Fail;
            response.Message = $"Cannot create duplicate line items for the same product. Duplicate Product ID(s): {string.Join(", ", allConflictingProductIds)}";
            return response;
        }

        if (duplicateProductIds.Any())
        {
            response.Status = ResponseStatus.Fail;
            response.Message = "Cannot create duplicate line items for the same product.";
            return response;
        }

        if (missingProductIds.Any())
        {
            response.Status = ResponseStatus.Fail;
            response.Message = $"Invalid Product ID(s): {string.Join(", ", missingProductIds)}";
            return response;
        }

        foreach (var incomingLineItem in updateSaleDto.LineItems)
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

        existingSale.DateAndTimeOfSale = updateSaleDto.DateAndTimeOfSale;
        existingSale.TotalPrice = existingSale.LineItems.Sum(li => li.Quantity * li.UnitPrice);

        response = await _saleRepository.UpdateSale(existingSale);

        return response;
    }

    public async Task<BaseResponse<Sale>> DeleteSale(int id)
    {
        var response = new BaseResponse<Sale>();

        response = await _saleRepository.GetSaleById(id);

        if (response.Status == ResponseStatus.Fail)
        {
            return response;
        }

        return await _saleRepository.DeleteSale(id);
    }
}
