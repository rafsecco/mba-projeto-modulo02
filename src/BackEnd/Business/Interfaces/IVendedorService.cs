using Business.Models;
using Business.ViewModels;

namespace Business.Interfaces;

public interface IVendedorService
{
	Task<List<Vendedor>> GetAsync(CancellationToken cancellationToken);
	Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken);
	Task<Guid?> CriaAsync(UserViewModel userViewModel, CancellationToken cancellationToken);
	Task<Vendedor> ObterVendedorPorIdAsync(Guid id, CancellationToken cancellationToken);
}
