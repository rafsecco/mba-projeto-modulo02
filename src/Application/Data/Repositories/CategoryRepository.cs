using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsValidCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .AnyAsync(p => p.Id == categoryId, cancellationToken);
    }
}

public interface ICategoryRepository : IRepository<Category>

{
    Task<bool> IsValidCategoryAsync(Guid categoryId, CancellationToken cancellationToken);
}