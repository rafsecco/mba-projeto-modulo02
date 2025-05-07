using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> HasProductsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .AnyAsync(p => p.CategoryId == categoryId, cancellationToken);
    }

    public async Task<List<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    public override Task<List<Product>> GetAsync(CancellationToken cancellationToken)
    {
        return _dbContext.Products.Include(p => p.Category).ToListAsync(cancellationToken);
    }

    public override async Task<Product> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<List<Product>> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken)
    {
        return await _dbContext.Products.Where(p => p.SellerId == sellerId).ToListAsync(cancellationToken);
    }
}

public interface IProductRepository : IRepository<Product>

{
    Task<bool> HasProductsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken);

    Task<List<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken);

    Task<List<Product>> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken);
}