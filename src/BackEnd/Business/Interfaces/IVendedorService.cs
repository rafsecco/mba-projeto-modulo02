using Business.Models;

namespace Business.Interfaces;

public interface IVendedorService
{
    Task<List<Vendedor>> GetAsync(CancellationToken cancellationToken);
}