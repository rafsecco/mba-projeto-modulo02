using Business.Models;

namespace Business.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken);

        Task<Favorito> AddFavoritoAsync(Guid clienteId, Guid produtoId, CancellationToken cancellationToken);

        Task RemoveFavoritoAsync(Guid clienteId, Guid produtoId, CancellationToken cancellationToken);

        Task<List<Favorito>> GetFavoritosAsync(Guid clienteId, CancellationToken cancellationToken);
    }
}