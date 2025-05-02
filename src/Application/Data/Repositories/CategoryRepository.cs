using Core.Domain.Entities;

namespace Core.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }

    public interface ICategoryRepository : IRepository<Category>

    {
    }
}