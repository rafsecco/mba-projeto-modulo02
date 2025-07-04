using Business.Models;

namespace Business.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>

{
    Task<bool> IsValidCategoryAsync(Guid categoriaId, CancellationToken cancellationToken);
}