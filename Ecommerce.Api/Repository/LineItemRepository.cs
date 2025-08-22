using Ecommerce.Api.Models;
using Ecommerce.Api.Data;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Repository;

public class LineItemRepository : ILineItemRepository
{
    private readonly EcommerceDbContext _dbContext;

    public LineItemRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BaseResponse<List<LineItem>>> GetAllLineItems()
    {
        var response = new BaseResponse<List<LineItem>>();

        try
        {
            var lineItems = await _dbContext.LineItems
                .Include(li => li.Sale)
                .Include(li => li.Product)
                .ThenInclude(li => li.Category)
                .ToListAsync();

            response.Status = ResponseStatus.Success;
            response.Data = lineItems;
        }
        catch (Exception ex)
        {
            response.Message = $"Error in LineItemRepository {nameof(GetAllLineItems)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<LineItem>> GetLineItemById(int id)
    {
            var response = new BaseResponse<LineItem>();

        try
        {
            var lineItem = await _dbContext.LineItems
                .Include(li => li.Sale)
                .Include(li => li.Product)
                .ThenInclude(li => li.Category)
                .FirstOrDefaultAsync(li => li.LineItemId == id);

            if (lineItem == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Line Item not found.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = lineItem;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in LineItemRepository {nameof(GetLineItemById)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<LineItem>> CreateLineItem(LineItem lineItem)
    {
        var response = new BaseResponse<LineItem>();

        try
        {
            _dbContext.LineItems.Add(lineItem);

            await _dbContext.SaveChangesAsync();

            if (lineItem == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Line Item not created.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = lineItem;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in LineItemRepository {nameof(CreateLineItem)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<LineItem>> UpdateLineItem(LineItem updatedLineItem)
    {
            var response = new BaseResponse<LineItem>();

        //    try
        //    {
        //        _dbContext.Products.Update(updatedProduct);
        //        var affectedRows = await _dbContext.SaveChangesAsync();

        //        if (affectedRows == 0)
        //        {
        //            response.Status = ResponseStatus.Fail;
        //            response.Message = "No changes were saved.";
        //        }
        //        else
        //        {
        //            await _dbContext.Entry(updatedProduct).Reference(p => p.Category).LoadAsync();

        //            response.Status = ResponseStatus.Success;
        //            response.Data = updatedProduct;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = $"Error in ProductRepository {nameof(UpdateProduct)}: {ex.Message}";
        //        response.Status = ResponseStatus.Fail;
        //    }

        return response;
    }

    public async Task<BaseResponse<LineItem>> DeleteLineItem(int id)
    {
            var response = new BaseResponse<LineItem>();

        //    try
        //    {
        //        response = await GetProductById(id);

        //        response.Data.IsDeleted = true;

        //        _dbContext.Products.Update(response.Data);

        //        var affectedRows = await _dbContext.SaveChangesAsync();

        //        if (affectedRows == 0)
        //        {
        //            response.Status = ResponseStatus.Fail;
        //            response.Message = "Deletion failed.";
        //        }
        //        else
        //        {
        //            response.Status = ResponseStatus.Success;
        //            response.Message = "Product deleted.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = $"Error in ProductRepository {nameof(DeleteProduct)}: {ex.Message}";
        //        response.Status = ResponseStatus.Fail;
        //    }

        return response;
    }
}
