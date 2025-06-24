using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repositories;

public class SellerRepository : ISellerRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SellerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateAsync(Seller seller, CancellationToken cancellationToken)
    {
        await _dbContext.Sellers.AddAsync(seller, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return seller.UserId;
    }

	public Task<List<Seller>> GetAsync(CancellationToken cancellationToken)
	{
		var retorno = _dbContext.Sellers.ToListAsync(cancellationToken);
		return retorno;
	}
}

public interface ISellerRepository
{
    public Task<Guid> CreateAsync(Seller seller, CancellationToken cancellationToken);
	public Task<List<Seller>> GetAsync(CancellationToken cancellationToken);
}
