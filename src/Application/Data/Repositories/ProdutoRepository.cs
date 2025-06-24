using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> HasProdutosByCategoriaIdAsync(Guid categoriaId, CancellationToken cancellationToken)
    {
        return await _dbContext.Produtos
            .AsNoTracking()
            .AnyAsync(p => p.CategoriaId == categoriaId, cancellationToken);
    }

    public async Task<List<Produto>> GetByCategoriaIdAsync(Guid categoriaId, CancellationToken cancellationToken)
    {
        return await _dbContext.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.CategoriaId == categoriaId)
            .ToListAsync(cancellationToken);
    }

    public override async Task<List<Produto>> GetAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Produtos.Include(p => p.Categoria).ToListAsync(cancellationToken);
    }

    public override async Task<Produto> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Produtos
            .Include(p => p.Categoria)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<List<Produto>> GetByVendedorIdAsync(Guid vendedorId, CancellationToken cancellationToken)
    {
        return await _dbContext.Produtos.Where(p => p.VendedorId == vendedorId).ToListAsync(cancellationToken);
    }
}

public interface IProdutoRepository : IRepository<Produto>

{
    Task<bool> HasProdutosByCategoriaIdAsync(Guid categoriaId, CancellationToken cancellationToken);

    Task<List<Produto>> GetByCategoriaIdAsync(Guid categoriaId, CancellationToken cancellationToken);

    Task<List<Produto>> GetByVendedorIdAsync(Guid vendedorId, CancellationToken cancellationToken);
}