using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios;

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
            .Include(p => p.Vendedor)
            .Where(p => p.CategoriaId == categoriaId && p.Ativo && p.Vendedor.Ativo)
            .ToListAsync(cancellationToken);
    }

    public override async Task<List<Produto>> GetAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Produtos.Include(p => p.Categoria).ToListAsync(cancellationToken);
    }

    public async Task<List<Produto>> GetValidProductsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Produtos
            .Include(p => p.Categoria)
            .Include(p => p.Vendedor)
            .Where(p => p.Vendedor != null && p.Vendedor.Ativo && p.Ativo)
            .ToListAsync(cancellationToken);
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
    public async Task<List<Produto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Produtos.ToListAsync(cancellationToken);
    }
}
