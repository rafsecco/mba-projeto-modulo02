using Business.Models;

namespace Business.Interfaces;

public interface IProdutoRepository : IRepository<Produto>

{
    Task<bool> HasProdutosByCategoriaIdAsync(Guid categoriaId, CancellationToken cancellationToken);

    Task<List<Produto>> GetByCategoriaIdAsync(Guid categoriaId, CancellationToken cancellationToken);

    Task<List<Produto>> GetByVendedorIdAsync(Guid vendedorId, CancellationToken cancellationToken);
}