using Core.Data.Repositories;
using Core.Domain.Entities;
using Core.ViewModels;

namespace Core.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

    public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    public async Task<List<Category>> GetAsync(CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetAsync(cancellationToken);
    }

    public async Task<Category> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _categoryRepository.FindAsync(id, cancellationToken);
    }

    public async Task<Guid> CreateAsync(CreateCategoryViewModel createCategoryViewModel, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = createCategoryViewModel.Name,
            Description = createCategoryViewModel.Description
        };

        return await _categoryRepository.CreateAsync(category, cancellationToken);
    }

    public async Task UpdateAsync(UpdateCategoryViewModel updateCategoryViewModel, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.FindAsync(updateCategoryViewModel.Id, cancellationToken);
        if (category is null) throw new KeyNotFoundException("Categoria não encontrada");
        category.Name = updateCategoryViewModel.Name ?? category.Name;
        category.Description = updateCategoryViewModel.Description ?? category.Description;

        await _categoryRepository.UpdateAsync(category, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var hasProducts = await _productRepository.HasProductsByCategoryIdAsync(id, cancellationToken);
        if (!hasProducts) await _categoryRepository.DeleteAsync(id, cancellationToken);
        return !hasProducts;
    }

    public async Task<bool> IsValidCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _categoryRepository.IsValidCategoryAsync(categoryId, cancellationToken);
    }
}

public interface ICategoryService
{
    public Task<List<Category>> GetAsync(CancellationToken cancellationToken);

    public Task<Category> FindAsync(Guid id, CancellationToken cancellationToken);

    public Task<Guid> CreateAsync(CreateCategoryViewModel createCategoryViewModel, CancellationToken cancellationToken);

    public Task UpdateAsync(UpdateCategoryViewModel updateCategoryViewModel, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> IsValidCategoryAsync(Guid categoryId, CancellationToken cancellationToken);
}