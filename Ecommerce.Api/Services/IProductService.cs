using Ecommerce.Api.Models;

namespace Ecommerce.Api.Services;

public interface IProductService
{
    Task<List<Product>> GetAllProducts();

    Task<Product> GetProductById(int id);

    void CreateProduct(Product product);

    void UpdateProduct(Product product);

    void DeleteProduct(int id);
}
