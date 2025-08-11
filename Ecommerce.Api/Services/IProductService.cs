using Ecommerce.Api.Models;

namespace Ecommerce.Api.Services;

public interface IProductService
{
    public List<Product> GetProducts();

    public Product GetProductById(int id);

    public Product CreateProduct(Product product);

    public Product UpdateProduct(Product updatedProduct);

    public Product DeleteProduct(int id);
}
