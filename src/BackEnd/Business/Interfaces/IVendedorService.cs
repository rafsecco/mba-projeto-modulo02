using Business.Models;

namespace Business.Interfaces;

public interface IVendedorService
{
	Task<List<Vendedor>> GetAsync(CancellationToken cancellationToken);
	Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken);
	Task<Guid?> CriaAsync(Guid id, CancellationToken cancellationToken);
	Task<Vendedor> ObterVendedorPorIdAsync(Guid id, CancellationToken cancellationToken);
}
