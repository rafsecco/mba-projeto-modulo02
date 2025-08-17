using Business.Interfaces;
using Business.Models;
using Business.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly Guid _currentUserId;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClienteService(IClienteRepository clienteRepository, IHttpContextAccessor httpContextAccessor)
        {
            _clienteRepository = clienteRepository;
            _httpContextAccessor = httpContextAccessor;

            _currentUserId = GetCurrentUserId();
        }

        public async Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken)
        {
            await _clienteRepository.AtualizaAtivoAsync(id, ativo, cancellationToken);
        }

        public async Task<Guid?> CriaAsync(UserViewModel userViewModel, Guid identityId, CancellationToken cancellationToken)
        {
            Cliente cliente = new Cliente { Ativo = true, Id = identityId, UserId = identityId };
            var clienteId = await _clienteRepository.CreateAsync(cliente, cancellationToken);
            return clienteId;
        }

        public async Task<List<Cliente>> GetAsync(CancellationToken cancellationToken)
        {
            var retorno = await _clienteRepository.GetAsync(cancellationToken);
            return retorno;
        }

        public async Task<Favorito> AddFavoritoAsync(Guid produtoId, CancellationToken cancellationToken)
        {
            return await _clienteRepository.AddFavoritoAsync(_currentUserId, produtoId, cancellationToken);
        }

        public async Task RemoveFavoritoAsync(Guid produtoId, CancellationToken cancellationToken)
        {
            await _clienteRepository.RemoveFavoritoAsync(_currentUserId, produtoId, cancellationToken);
        }

        public async Task<List<Favorito>> GetFavoritosAsync(CancellationToken cancellationToken)
        {
            return await _clienteRepository.GetFavoritosAsync(_currentUserId, cancellationToken);
        }

        //TODO: adicionar em outro lugar para não repetir código
        private Guid GetCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated != true)
                return Guid.Empty;

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("Usuário não encontrado");

            return Guid.Parse(userId);
        }
    }
}
