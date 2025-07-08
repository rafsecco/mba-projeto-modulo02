using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios
{
    public class ClienteRepository : IClienteRepository
        
    {
        private readonly ApplicationDbContext _dbContext;
        public ClienteRepository(ApplicationDbContext dbContext)
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
            await  _dbContext.SaveChangesAsync(cancellationToken);

        }

        public async Task<Guid> CreateAsync(Guid userId, CancellationToken cancellationToken)
        {
            Cliente cliente = new Cliente
            {
                UserId = userId,
                Ativo = true
            };
            _dbContext.Clientes.Add(cliente);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return cliente.UserId;
        }

        public async Task<List<Cliente>> GetAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Clientes.ToListAsync(cancellationToken);
        }
    }
}


