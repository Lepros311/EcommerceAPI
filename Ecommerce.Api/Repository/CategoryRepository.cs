using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly EcommerceDbContext _dbContext;

    public CategoryRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Category>> GetAllCategories()
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponse<Category>> GetCategoryById(int id)
    {
        var response = new BaseResponse<Category>();

        try
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Category not found.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = category;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in CategoryRepository {nameof(CategoryRepository)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public Task<Category> CreateCategory(Category category)
    {
        throw new NotImplementedException();
    }

    public Task<Category> UpdateCategory(int id, Category updatedCategory)
    {
        throw new NotImplementedException();
    }

    public Task<string> DeleteCategory(int id)
    {
        throw new NotImplementedException();
    }
}
