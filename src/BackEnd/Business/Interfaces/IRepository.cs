using Business.Models;

namespace Business.Interfaces;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    public Task<Guid> CreateAsync(TEntity entity, CancellationToken cancellationToken);

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    public Task<List<TEntity>> GetAsync(CancellationToken cancellationToken);

    public Task<TEntity> FindAsync(Guid id, CancellationToken cancellationToken);

	public Task AtivarAsync(TEntity entity, CancellationToken cancellationToken);
	public Task InativarAsync(TEntity entity, CancellationToken cancellationToken);
}
