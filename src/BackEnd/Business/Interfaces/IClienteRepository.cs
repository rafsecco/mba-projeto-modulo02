using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Guid> CreateAsync(Guid userId, CancellationToken cancellationToken);

        Task<List<Cliente>> GetAsync(CancellationToken cancellationToken);

        Task AtualizaAtivoAsync(Guid id, bool ativo, CancellationToken cancellationToken);        
    }
}
