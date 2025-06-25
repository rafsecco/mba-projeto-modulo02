using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repositories;

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

public interface IVendedorRepository
{
    public Task<Guid> CreateAsync(Vendedor vendedor, CancellationToken cancellationToken);

    public Task<List<Vendedor>> GetAsync(CancellationToken cancellationToken);
}