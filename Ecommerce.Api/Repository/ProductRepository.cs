using Ecommerce.Api.Models;
using Ecommerce.Api.Data;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Repository;

public class ProductRepository : IProductRepository
{
    private readonly EcommerceDbContext _dbContext;

    public ProductRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BaseResponse<List<Product>>> GetAllProducts()
    {
        var response = new BaseResponse<List<Product>>();

        try
        {
            var products = await _dbContext.Products.Include(p => p.Category).ToListAsync();

            response.Status = ResponseStatus.Success;
            response.Data = products;
        }
        catch (Exception ex)
        {
            response.Message = $"Error in ProductRepository {nameof(GetAllProducts)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }
        
        return response;
    }

    public async Task<BaseResponse<Product>> GetProductById(int id)
    {
        var response = new BaseResponse<Product>();

        try
        {
            var product = await _dbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Product not found.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = product;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in ProductRepository {nameof(GetProductById)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Product>> CreateProduct(Product product)
    {
        var response = new BaseResponse<Product>();

        try
        {
            _dbContext.Products.Add(product);

            await _dbContext.SaveChangesAsync();

            if (product == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Product not created.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = product;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in ProductRepository {nameof(CreateProduct)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Product>> UpdateProduct(Product updatedProduct)
    {
        var response = new BaseResponse<Product>();

        try
        {
            _dbContext.Products.Update(updatedProduct);
            var affectedRows = await _dbContext.SaveChangesAsync();

            if (affectedRows == 0)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "No changes were saved.";
            }
            else
            {
                await _dbContext.Entry(updatedProduct).Reference(p => p.Category).LoadAsync();

                response.Status = ResponseStatus.Success;
                response.Data = updatedProduct;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in ProductRepository {nameof(UpdateProduct)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Product>> DeleteProduct(int id)
    {
        var response = new BaseResponse<Product>();

        try
        {
            response = await GetProductById(id);

            response.Data.IsDeleted = true;

            _dbContext.Products.Update(response.Data);

            var affectedRows = await _dbContext.SaveChangesAsync();

            if (affectedRows == 0)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Deletion failed.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Message = "Product deleted.";
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in ProductRepository {nameof(DeleteProduct)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }
}
