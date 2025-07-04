using Business.Data.Repositories;
using Business.Domain.Entities;

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
}

public interface IVendedorService
{
    Task<List<Vendedor>> GetAsync(CancellationToken cancellationToken);
}