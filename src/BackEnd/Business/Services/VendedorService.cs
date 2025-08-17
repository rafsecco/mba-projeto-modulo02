using Business.Interfaces;
using Business.Models;

namespace Business.Services;

public class VendedorService : IVendedorService
{
    private readonly IVendedorRepository _vendedorRepository;

    public VendedorService(IVendedorRepository vendedorRepository)
    {
        _vendedorRepository = vendedorRepository;
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

    public async Task<Guid?> CriaAsync(Guid id, CancellationToken cancellationToken)
    {
		await _vendedorRepository.CreateAsync(id, cancellationToken);
       return id;
    }

	public async Task<Vendedor> ObterVendedorPorIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var retorno = await _vendedorRepository.ObterVendedorPorIdAsync(id, cancellationToken);
		return retorno;
	}
}
