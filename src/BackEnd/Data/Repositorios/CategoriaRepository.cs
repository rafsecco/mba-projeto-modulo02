using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios;

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