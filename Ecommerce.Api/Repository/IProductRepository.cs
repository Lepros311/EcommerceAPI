﻿using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Repository;

public interface IProductRepository
{
    public Task<PagedResponse<List<Product>>> GetAllProducts(PaginationParams paginationParams);

    public Task<BaseResponse<Product>> GetProductById(int id);

    public Task<BaseResponse<Product>> CreateProduct(Product product);

    public Task<BaseResponse<Product>> UpdateProduct(Product updatedProduct);

    public Task<BaseResponse<Product>> DeleteProduct(int id);
}
