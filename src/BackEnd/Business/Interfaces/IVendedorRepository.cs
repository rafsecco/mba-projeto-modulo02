using Business.Models;

namespace Business.Interfaces;

public interface IVendedorRepository : IRepository<Vendedor>
{
    Task<Guid> CreateAsync(Guid userId, CancellationToken cancellationToken);

    Task<List<Vendedor>> GetAsync(CancellationToken cancellationToken);

    Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken);

	Task<Vendedor> ObterVendedorPorIdAsync(Guid id, CancellationToken cancellationToken);
}
