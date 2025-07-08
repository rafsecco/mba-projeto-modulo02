using Business.Interfaces;
using Business.Models;
using Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IUserService _userService;
        public ClienteService(IClienteRepository clienteRepository, IUserService userService)
        {
            _clienteRepository = clienteRepository;
            _userService = userService;
        }
        public async Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken)
        {
            await _clienteRepository.AtualizaAtivoAsync(id, ativo, cancellationToken);
        }

        public async Task<Guid?> CriaAsync(UserViewModel userViewModel, CancellationToken cancellationToken)
        {
            var userId = _userService.RegisterAsync(userViewModel, "cliente", cancellationToken);
            await _clienteRepository.CreateAsync(userId.Result.Value, cancellationToken);
            return userId.Result.Value;
        }

        public async Task<List<Cliente>> GetAsync(CancellationToken cancellationToken)
        {
            var retorno = await _clienteRepository.GetAsync(cancellationToken);
            return retorno;
        }
    }
}
