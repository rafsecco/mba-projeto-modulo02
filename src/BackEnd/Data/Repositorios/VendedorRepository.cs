using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios;

public class VendedorRepository : IVendedorRepository
{
    private readonly ApplicationDbContext _dbContext;

    public VendedorRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateAsync(Vendedor vendedor, CancellationToken cancellationToken)
    {
        await _dbContext.Vendedores.AddAsync(vendedor, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return vendedor.UserId;
    }

    public Task<List<Vendedor>> GetAsync(CancellationToken cancellationToken)
    {
        var retorno = _dbContext.Vendedores.ToListAsync(cancellationToken);
        return retorno;
    }
}