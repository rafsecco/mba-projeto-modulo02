using Business.Models;
using Business.ViewModels;

namespace Business.Interfaces;

public interface ICategoriaService
{
    public Task<List<Categoria>> GetAsync(CancellationToken cancellationToken);

    public Task<Categoria> FindAsync(Guid id, CancellationToken cancellationToken);

    public Task<Guid> CreateAsync(CreateCategoriaViewModel createCategoriaViewModel, CancellationToken cancellationToken);

    public Task UpdateAsync(UpdateCategoriaViewModel updateCategoriaViewModel, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> IsValidCategoryAsync(Guid categoriaId, CancellationToken cancellationToken);
}