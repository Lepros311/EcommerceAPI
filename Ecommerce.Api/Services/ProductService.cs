using Ecommerce.Api.Models;
using Ecommerce.Api.Data;

namespace Ecommerce.Api.Services;

public class ProductService : IProductService
{
    private readonly EcommerceDbContext _dbContext;

    public ProductService(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Product CreateProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public Product DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }

    public Product GetProductById(int id)
    {
        throw new NotImplementedException();
    }

    public List<Product> GetProducts()
    {
        return _dbContext.Products.ToList();
    }

    public Product UpdateProduct(Product updatedProduct)
    {
        throw new NotImplementedException();
    }
}
