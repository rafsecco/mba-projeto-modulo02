using Business.Models;
using Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IClienteService
    {
        Task<List<Cliente>> GetAsync(CancellationToken cancellationToken);

        Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken);

        Task<Guid?> CriaAsync(UserViewModel userViewModel, CancellationToken cancellationToken);

        Task<Favorito> AddFavoritoAsync(Guid produtoId, CancellationToken cancellationToken);

        Task RemoveFavoritoAsync(Guid produtoId, CancellationToken cancellationToken);

        Task<List<Favorito>> GetFavoritosAsync(CancellationToken cancellationToken);
    }
}