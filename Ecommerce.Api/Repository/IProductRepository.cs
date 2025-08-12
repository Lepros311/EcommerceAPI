using Ecommerce.Api.Models;

namespace Ecommerce.Api.Repository;

public interface IProductRepository
{
    public Task<List<Product>> GetAllProducts();

    public Task<Product> GetProductById(int id);

    public Task<Product> CreateProduct(Product product);

    public Task<Product> UpdateProduct(int id, Product updatedProduct);

    public Task<string> DeleteProduct(int id);
}
