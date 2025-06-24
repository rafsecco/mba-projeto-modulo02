using Core.Data.Repositories;
using Core.Domain.Entities;
using Core.ViewModels;

namespace Core.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IProdutoRepository _produtoRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository, IProdutoRepository produtoRepository)
    {
        _categoriaRepository = categoriaRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<List<Categoria>> GetAsync(CancellationToken cancellationToken)
    {
        return await _categoriaRepository.GetAsync(cancellationToken);
    }

    public async Task<Categoria> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _categoriaRepository.FindAsync(id, cancellationToken);
    }

    public async Task<Guid> CreateAsync(CreateCategoriaViewModel createCategoriaViewModel, CancellationToken cancellationToken)
    {
        var categoria = new Categoria
        {
            Nome = createCategoriaViewModel.Nome,
            Descricao = createCategoriaViewModel.Descricao
        };

        return await _categoriaRepository.CreateAsync(categoria, cancellationToken);
    }

    public async Task UpdateAsync(UpdateCategoriaViewModel updateCategoriaViewModel, CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepository.FindAsync(updateCategoriaViewModel.Id, cancellationToken);
        if (categoria is null) throw new KeyNotFoundException("Categoria não encontrada");
        categoria.Nome = updateCategoriaViewModel.Nome ?? categoria.Nome;
        categoria.Descricao = updateCategoriaViewModel.Descricao ?? categoria.Descricao;

        await _categoriaRepository.UpdateAsync(categoria, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var hasProdutos = await _produtoRepository.HasProdutosByCategoriaIdAsync(id, cancellationToken);
        if (!hasProdutos) await _categoriaRepository.DeleteAsync(id, cancellationToken);
        return !hasProdutos;
    }

    public async Task<bool> IsValidCategoryAsync(Guid categoriaId, CancellationToken cancellationToken)
    {
        return await _categoriaRepository.IsValidCategoryAsync(categoriaId, cancellationToken);
    }
}

public interface ICategoriaService
{
    public Task<List<Categoria>> GetAsync(CancellationToken cancellationToken);

    public Task<Categoria> FindAsync(Guid id, CancellationToken cancellationToken);

    public Task<Guid> CreateAsync(CreateCategoriaViewModel createCategoriaViewModel, CancellationToken cancellationToken);

    public Task UpdateAsync(UpdateCategoriaViewModel updateCategoriaViewModel, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> IsValidCategoryAsync(Guid categoriaId, CancellationToken cancellationToken);
}