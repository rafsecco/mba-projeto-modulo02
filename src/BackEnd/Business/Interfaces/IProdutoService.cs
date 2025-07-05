using Business.Models;
using Business.ViewModels;

namespace Business.Interfaces;

public interface IProdutoService
{
    Task<List<Produto>> GetAsync(CancellationToken cancellationToken);

    Task<List<Produto>> GetByVendedorId(CancellationToken cancellationToken);

    Task<Produto> FindAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Produto>> GetByCategoriaIdAsync(Guid categoriaId, CancellationToken cancellationToken);

    Task<Guid> CreateAsync(CriaProdutoViewModel criaProdutoViewModel, CancellationToken cancellationToken,
        string? path = null);

    Task UpdateAsync(AtualizaProdutoViewModel atualizaProdutoViewModel, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

	Task<List<Produto>> GetAllAsync(CancellationToken cancellationToken);

	Task<Guid> AtivarAsync(Guid id, CancellationToken cancellationToken);

	Task<Guid> InativarAsync(Guid id, CancellationToken cancellationToken);
}
