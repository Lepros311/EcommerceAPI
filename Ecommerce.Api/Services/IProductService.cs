using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public interface IProductService
{
    Task<List<Product>> GetAllProducts();

    Task<Product> GetProductById(int id);

    Task<BaseResponse<Product>> CreateProduct(Product product);

    void UpdateProduct(Product product);

    void DeleteProduct(int id);
}
