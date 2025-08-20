using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Repository;

public class SaleRepository : ISaleRepository
{
    private readonly EcommerceDbContext _dbContext;

    public SaleRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BaseResponse<List<Sale>>> GetAllSales()
    {
        var response = new BaseResponse<List<Sale>>();

        try
        {
            var sales = await _dbContext.Sales
                .Include(s => s.LineItems)
                .ThenInclude(li => li.Product)
                .ThenInclude(p => p.Category)
                .ToListAsync();

            response.Status = ResponseStatus.Success;
            response.Data = sales;
        }
        catch (Exception ex)
        {
            response.Message = $"Error in SaleRepository {nameof(GetAllSales)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Sale>> GetSaleById(int id)
    {
        var response = new BaseResponse<Sale>();

        try
        {
            var sale = await _dbContext.Sales
                .Include(s => s.LineItems)
                .ThenInclude(li => li.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(s => s.SaleId == id);

            if (sale == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Sale not found.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = sale;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in SaleRepository {nameof(SaleRepository)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Sale>> CreateSale(Sale sale)
    {
        var response = new BaseResponse<Sale>();

        try
        {
            _dbContext.Sales.Add(sale);

            await _dbContext.SaveChangesAsync();

            if (sale == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Sale not created.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = sale;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in SaleRepository {nameof(CreateSale)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Sale>> UpdateSale(Sale updatedSale)
    {
        var response = new BaseResponse<Sale>();

        try
        {
            _dbContext.Sales.Update(updatedSale);
            var affectedRows = await _dbContext.SaveChangesAsync();

            if (affectedRows == 0)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "No changes were saved.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = updatedSale;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in SaleRepository {nameof(UpdateSale)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Sale>> DeleteSale(int id)
    {
        var response = new BaseResponse<Sale>();

        //        try
        //        {
        //            response = await GetCategoryById(id);

        //            response.Data.IsDeleted = true;

        //            _dbContext.Categories.Update(response.Data);
        //            var affectedRows = await _dbContext.SaveChangesAsync();

        //            if (affectedRows == 0)
        //            {
        //                response.Status = ResponseStatus.Fail;
        //                response.Message = "Deletion failed.";
        //            }
        //            else
        //            {
        //                response.Status = ResponseStatus.Success;
        //                response.Message = "Category deleted.";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            response.Message = $"Error in CategoryRepository {nameof(DeleteCategory)}: {ex.Message}";
        //            response.Status = ResponseStatus.Fail;
        //        }

        return response;
    }
}

