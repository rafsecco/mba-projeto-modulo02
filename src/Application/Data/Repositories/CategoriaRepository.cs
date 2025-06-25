using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsValidCategoryAsync(Guid categoriaId, CancellationToken cancellationToken)
    {
        return await _dbContext.Categorias
            .AsNoTracking()
            .AnyAsync(p => p.Id == categoriaId, cancellationToken);
    }
}

public interface ICategoriaRepository : IRepository<Categoria>

{
    Task<bool> IsValidCategoryAsync(Guid categoriaId, CancellationToken cancellationToken);
}