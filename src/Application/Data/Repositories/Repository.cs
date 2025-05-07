using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        protected Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public virtual async Task<Guid> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _dbSet.Remove(new TEntity { Id = id });
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<List<TEntity>> GetAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public virtual async Task<TEntity> FindAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }

    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        public Task<Guid> CreateAsync(TEntity entity, CancellationToken cancellationToken);

        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        public Task<List<TEntity>> GetAsync(CancellationToken cancellationToken);

        public Task<TEntity> FindAsync(Guid id, CancellationToken cancellationToken);
    }
}