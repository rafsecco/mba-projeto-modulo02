using Core.Data.Repositories;
using Core.Domain.Entities;
using Core.ViewModels;

namespace Core.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IProductRepository _productRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository, IProductRepository productRepository)
    {
        _categoriaRepository = categoriaRepository;
        _productRepository = productRepository;
    }

    public async Task<List<Categoria>> GetAsync(CancellationToken cancellationToken)
    {
        return await _categoriaRepository.GetAsync(cancellationToken);
    }

    public async Task<Categoria> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _categoriaRepository.FindAsync(id, cancellationToken);
    }

    public async Task<Guid> CreateAsync(CriaCategoriaViewModel criaCategoriaViewModel, CancellationToken cancellationToken)
    {
        var category = new Categoria
        {
            Nome = criaCategoriaViewModel.Name,
            Descricao = criaCategoriaViewModel.Description
        };

        return await _categoriaRepository.CreateAsync(category, cancellationToken);
    }

    public async Task UpdateAsync(AtualizaCategoriaViewModel atualizaCategoriaViewModel, CancellationToken cancellationToken)
    {
        var category = await _categoriaRepository.FindAsync(atualizaCategoriaViewModel.Id, cancellationToken);
        if (category is null) throw new KeyNotFoundException("Categoria não encontrada");
        category.Nome = atualizaCategoriaViewModel.Name ?? category.Nome;
        category.Descricao = atualizaCategoriaViewModel.Description ?? category.Descricao;

        await _categoriaRepository.UpdateAsync(category, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var hasProducts = await _productRepository.HasProductsByCategoryIdAsync(id, cancellationToken);
        if (!hasProducts) await _categoriaRepository.DeleteAsync(id, cancellationToken);
        return !hasProducts;
    }

    public async Task<bool> IsValidCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _categoriaRepository.IsValidCategoryAsync(categoryId, cancellationToken);
    }
}

public interface ICategoriaService
{
    public Task<List<Categoria>> GetAsync(CancellationToken cancellationToken);

    public Task<Categoria> FindAsync(Guid id, CancellationToken cancellationToken);

    public Task<Guid> CreateAsync(CriaCategoriaViewModel criaCategoriaViewModel, CancellationToken cancellationToken);

    public Task UpdateAsync(AtualizaCategoriaViewModel atualizaCategoriaViewModel, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> IsValidCategoryAsync(Guid categoryId, CancellationToken cancellationToken);
}