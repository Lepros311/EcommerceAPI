using Ecommerce.Api.Models;
using Ecommerce.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Repository;

public class ProductRepository : IProductRepository
{
    private readonly EcommerceDbContext _dbContext;

    public ProductRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        var products = await _dbContext.Products.Include(p => p.Category).ToListAsync();

        return products;
    }

    public async Task<Product> GetProductById(int id)
    {
        var product = await _dbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);

        return product;
    }

    public async Task<Product> CreateProduct(Product product)
    {
        var category = await _dbContext.Categories.FindAsync(product.CategoryId);

        if (category == null)
        {
            throw new ArgumentException("Invalid CategoryId");
        }

        product.Category = category;
        _dbContext.Products.Add(product);

        await _dbContext.SaveChangesAsync();

        return product;
    }

    public Task<Product> UpdateProduct(int id, Product updatedProduct)
    {
        throw new NotImplementedException();
    }

    public Task<string> DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }
}
