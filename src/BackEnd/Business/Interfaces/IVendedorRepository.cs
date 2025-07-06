using Business.Models;

namespace Business.Interfaces;

public interface IVendedorRepository
{
    Task<Guid> CreateAsync(Vendedor vendedor, CancellationToken cancellationToken);

    Task<List<Vendedor>> GetAsync(CancellationToken cancellationToken);

    Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken);
}