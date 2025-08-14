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

    public async Task<BaseResponse<Product>> UpdateProduct(Product existingProduct)
    {
        var response = new BaseResponse<Product>();

        try
        {
            _dbContext.Products.Update(existingProduct);

            await _dbContext.SaveChangesAsync();

            if (existingProduct == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Product not created.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = existingProduct;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in ProductRepository {nameof(UpdateProduct)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public Task<string> DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }
}
