using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios;

public class ClienteRepository : Repository<Cliente>, IClienteRepository

{
    private readonly ApplicationDbContext _dbContext;

    public ClienteRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken)
    {
        var cliente = await _dbContext.Clientes.FindAsync(id, cancellationToken);
        if (cliente is null) return;
        cliente.Ativo = ativo;
        _dbContext.Clientes.Update(cliente);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Favorito> AddFavoritoAsync(Guid clienteId, Guid produtoId, CancellationToken cancellationToken)
    {
        var favorito = new Favorito
        {
            ClienteId = clienteId,
            ProdutoId = produtoId
        };

        _dbContext.Favoritos.Add(favorito);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return favorito;
    }

    public async Task RemoveFavoritoAsync(Guid clienteId, Guid produtoId, CancellationToken cancellationToken)
    {
        var favorito = new Favorito
        {
            ClienteId = clienteId,
            ProdutoId = produtoId
        };
        _dbContext.Favoritos.Remove(favorito);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<List<Favorito>> GetFavoritosAsync(Guid clienteId, CancellationToken cancellationToken)
    {
        return _dbContext.Favoritos
            .AsNoTrackingWithIdentityResolution()
            .Include(e => e.Produto)
            .Where(f => f.ClienteId == clienteId)
            .ToListAsync(cancellationToken);
    }
}