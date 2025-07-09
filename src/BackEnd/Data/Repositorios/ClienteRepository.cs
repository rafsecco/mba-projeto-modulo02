using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositorios
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
        
    {
        private readonly ApplicationDbContext _dbContext;
        public ClienteRepository(ApplicationDbContext dbContext):base(dbContext) 
        {
            _dbContext = dbContext;

        }
        
        public async Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken)
        {
            var cliente = await _dbContext.Clientes.FindAsync(id, cancellationToken);
            if (cliente is null) return;
            cliente.Ativo = ativo;
            _dbContext.Clientes.Update(cliente);            
            await  _dbContext.SaveChangesAsync(cancellationToken);

        }              
    }
}


