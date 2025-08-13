using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Category> GetCategoryById(int id)
    {
        var category = await _dbContext.Categories.FindAsync(id);

        return category;
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
