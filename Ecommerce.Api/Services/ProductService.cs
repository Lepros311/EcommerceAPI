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

    public async Task<BaseResponse<List<Product>>> GetAllProducts()
    {
        return await _productRepository.GetAllProducts();
    }

    public async Task<BaseResponse<Product>> GetProductById(int id)
    {
        return await _productRepository.GetProductById(id);
    }

    public async Task<BaseResponse<Product>> CreateProduct(Product product)
    {
        var productResponse = new BaseResponse<Product>();

        var categoryResponse = await _categoryRepository.GetCategoryById(product.CategoryId);

        if (categoryResponse.Status == ResponseStatus.Fail)
        {
            productResponse.Status = ResponseStatus.Fail;
            productResponse.Message = "Category not found.";
            return productResponse;
        }

        product.Category = categoryResponse.Data;

        productResponse = await _productRepository.CreateProduct(product);

        return productResponse;
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
