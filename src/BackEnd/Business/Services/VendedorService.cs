using Business.Interfaces;
using Business.Models;
using Business.ViewModels;

namespace Business.Services;

public class VendedorService : IVendedorService
{
    private readonly IVendedorRepository _vendedorRepository;
    private readonly IUserService _userService;

    public VendedorService(IVendedorRepository vendedorRepository, IUserService userService)
    {
        _vendedorRepository = vendedorRepository;
        _userService = userService;
    }

    public async Task<List<Vendedor>> GetAsync(CancellationToken cancellationToken)
    {
        var retorno = await _vendedorRepository.GetAsync(cancellationToken);
        return retorno;
    }

    public async Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken)
    {
       
        await _vendedorRepository.AtualizaAtivoAsync(id, ativo, cancellationToken);
    }

    public async Task<Guid?> CriaAsync(UserViewModel userViewModel, CancellationToken cancellationToken)
    {
       var userId = _userService.RegisterAsync(userViewModel, "Vendedor", cancellationToken);
       await _vendedorRepository.CreateAsync(userId.Result.Value, cancellationToken);
       return userId.Result.Value;
    }

	public async Task<Vendedor> ObterVendedorPorIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var retorno = await _vendedorRepository.ObterVendedorPorIdAsync(id, cancellationToken);
		return retorno;
	}
}
