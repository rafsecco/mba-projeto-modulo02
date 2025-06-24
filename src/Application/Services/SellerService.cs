using Core.Data.Repositories;
using Core.Domain.Entities;

namespace Core.Services;

public class SellerService : ISellerService
{
	private readonly ISellerRepository _sellerRepository;

	public SellerService(ISellerRepository sellerRepository)
	{
		_sellerRepository = sellerRepository;
	}

	public async Task<List<Seller>> GetAsync(CancellationToken cancellationToken)
	{
		var retorno = await _sellerRepository.GetAsync(cancellationToken);
		return retorno;
	}
}

public interface ISellerService
{
	Task<List<Seller>> GetAsync(CancellationToken cancellationToken);
}

