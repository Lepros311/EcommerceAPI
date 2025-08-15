using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public interface IProductService
{
    Task<BaseResponse<List<Product>>> GetAllProducts();

    Task<BaseResponse<Product>> GetProductById(int id);

    Task<BaseResponse<Product>> CreateProduct(Product product);

    Task<BaseResponse<Product>> UpdateProduct(Product product);

    Task<BaseResponse<Product>> DeleteProduct(int id);
}
