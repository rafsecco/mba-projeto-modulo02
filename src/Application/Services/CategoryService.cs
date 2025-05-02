using Core.Data.Repositories;
using Core.Domain.Entities;

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

    public async Task<Category> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Guid> CreateAsync(Category category, CancellationToken cancellationToken)
    {
        return await _categoryRepository.CreateAsync(category, cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        await _categoryRepository.UpdateAsync(category, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var hasProducts = await _productRepository.HasProductsByCategoryIdAsync(id, cancellationToken);
        if (!hasProducts) await _categoryRepository.DeleteAsync(id, cancellationToken);
        return !hasProducts;
    }
}

public interface ICategoryService
{
    public Task<List<Category>> GetAsync(CancellationToken cancellationToken);

    public Task<Category> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<Guid> CreateAsync(Category category, CancellationToken cancellationToken);

    public Task UpdateAsync(Category category, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}