using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios;

public class VendedorRepository : Repository<Vendedor>, IVendedorRepository
{
    public VendedorRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Guid> CreateAsync(Guid userId, CancellationToken cancellationToken)
    {
        var vendedor = new Vendedor
        {
            UserId = userId,
            Ativo = true
        };
        await _dbContext.Vendedores.AddAsync(vendedor, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return vendedor.UserId;
    }

    public Task<List<Vendedor>> GetAsync(CancellationToken cancellationToken)
    {
        var retorno = _dbContext.Vendedores.ToListAsync(cancellationToken);
        return retorno;
    }

    public async Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken)
    {
        var vendedor = await _dbContext.Vendedores.FindAsync(id, cancellationToken);
        if (vendedor is null) return;
        vendedor.Ativo = ativo;
        _dbContext.Vendedores.Update(vendedor);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}