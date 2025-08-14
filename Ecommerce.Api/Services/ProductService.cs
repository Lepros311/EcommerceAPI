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
        var response = await _productRepository.GetAllProducts();

        if (response.Data == null)
        {
            response.Status = ResponseStatus.Fail;
            response.Message = "No products found.";
            return response;
        }

        response.Status = ResponseStatus.Success;

        return response;
    }

    public async Task<BaseResponse<Product>> GetProductById(int id)
    {
        var response = await _productRepository.GetProductById(id);

        if (response.Data == null)
        {
            response.Status = ResponseStatus.Fail;
            response.Message = "Product not found.";
            return response;
        }

        response.Status = ResponseStatus.Success;

        return response;
    }

    public async Task<BaseResponse<Product>> CreateProduct(Product product)
    {
        var productResponse = new BaseResponse<Product>();

        var categoryResponse = await _categoryRepository.GetCategoryById(product.CategoryId);

        if (categoryResponse.Data == null)
        {
            productResponse.Status = ResponseStatus.Fail;
            productResponse.Message = "Category not found.";
            return productResponse;
        }

        product.Category = categoryResponse.Data;

        productResponse = await _productRepository.CreateProduct(product);

        if (productResponse.Data == null)
        {
            productResponse.Status = ResponseStatus.Fail;
            productResponse.Message = "Product not created.";
        }

        productResponse.Status = ResponseStatus.Success;

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
