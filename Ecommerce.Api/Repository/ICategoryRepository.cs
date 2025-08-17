﻿using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Repository;

public interface ICategoryRepository
{
    public Task<BaseResponse<List<Category>>> GetAllCategories();

    public Task<BaseResponse<Category>> GetCategoryById(int id);

    public Task<BaseResponse<Category>> CreateCategory(Category category);

    public Task<BaseResponse<Category>> UpdateCategory(Category updatedCategory);

    public Task<BaseResponse<Category>> DeleteCategory(int id);
}