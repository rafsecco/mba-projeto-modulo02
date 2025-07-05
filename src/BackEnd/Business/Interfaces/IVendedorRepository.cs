using Business.Models;

namespace Business.Interfaces;

public interface IVendedorRepository
{
    public Task<Guid> CreateAsync(Vendedor vendedor, CancellationToken cancellationToken);

    public Task<List<Vendedor>> GetAsync(CancellationToken cancellationToken);
}