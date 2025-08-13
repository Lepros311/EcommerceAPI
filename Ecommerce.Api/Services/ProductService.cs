using Ecommerce.Api.Models;
using Ecommerce.Api.Repository;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await _productRepository.GetAllProducts();
    }

    public async Task<Product> GetProductById(int id)
    {
        return await _productRepository.GetProductById(id);
    }

    public async Task<BaseResponse<Product>> CreateProduct(Product product)
    {
        var response = new BaseResponse<Product>();

        var category = await _categoryRepository.GetCategoryById(product.CategoryId);

        if (category == null)
        {
            response.Status = ResponseStatus.Fail;
            response.Message = "Category not found.";
        }

        product.Category = category;

        var createdProduct = await _productRepository.CreateProduct(product);

        if (createdProduct == null)
        {
            response.Status = ResponseStatus.Fail;
            response.Message = "Unable to create product.";
        }

        response.Status = ResponseStatus.Success;
        response.Data = createdProduct;

        return response;
    }

    public void UpdateProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public void DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }
}
