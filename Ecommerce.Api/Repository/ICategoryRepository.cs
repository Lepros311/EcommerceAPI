using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Repository;

public interface ICategoryRepository
{
    public Task<List<Category>> GetAllCategories();

    public Task<BaseResponse<Category>> GetCategoryById(int id);

    public Task<Category> CreateCategory(Category category);

    public Task<Category> UpdateCategory(int id, Category updatedCategory);

    public Task<string> DeleteCategory(int id);
}