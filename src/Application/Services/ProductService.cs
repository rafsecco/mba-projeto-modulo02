using Core.Data.Repositories;
using Core.Domain.Entities;
using Core.Utils;
using Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Core.Services;

public class ProductService : IProductService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly Guid _currentUserId;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProductRepository _productRepository;
    private readonly Upload _upload;

    public ProductService(IProductRepository productRepository, IHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor, ICategoriaRepository categoriaRepository, Upload upload)
    {
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
        _categoriaRepository = categoriaRepository;
        _upload = upload;
        _currentUserId = GetCurrentUserId();
    }

    public async Task<List<Product>> GetAsync(CancellationToken cancellationToken)
    {
        return await _productRepository.GetAsync(cancellationToken);
    }

    public async Task<List<Product>> GetBySellerId(CancellationToken cancellationToken)
    {
        return await _productRepository.GetBySellerIdAsync(_currentUserId, cancellationToken);
    }

    public async Task<Product> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _productRepository.FindAsync(id, cancellationToken);
    }

    public async Task<List<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _productRepository.GetByCategoryIdAsync(categoryId, cancellationToken);
    }

    public async Task<Guid> CreateAsync(CreateProductViewModel createProductViewModel,
        CancellationToken cancellationToken, string? path = null)
    {
        var isValidCategory =
            await _categoriaRepository.IsValidCategoryAsync(createProductViewModel.CategoryId, cancellationToken);
        if (!isValidCategory)
            throw new KeyNotFoundException("Categoria inválida");

        var product = new Product
        {
            Name = createProductViewModel.Name,
            Description = createProductViewModel.Description,
            Price = createProductViewModel.Price,
            Stock = createProductViewModel.Stock,
            CategoryId = createProductViewModel.CategoryId,
            SellerId = _currentUserId
        };
        if (createProductViewModel.UploadImage != null)
            await _upload.AddImageAsync(product, createProductViewModel.UploadImage, path, cancellationToken);

        return await _productRepository.CreateAsync(product, cancellationToken);
    }

    public async Task UpdateAsync(UpdateProductViewModel updateProductViewModel, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindAsync(updateProductViewModel.Id, cancellationToken);
        if (product is null) throw new KeyNotFoundException("Produto não encontrado");

        if (!IsUserOwner(product))
            throw new UnauthorizedAccessException("Ação não permitida.");

        product.Name = updateProductViewModel.Name ?? product.Name;
        product.Description = updateProductViewModel.Description ?? product.Description;
        product.Price = updateProductViewModel.Price ?? product.Price;

        product.Stock = updateProductViewModel.Stock ?? product.Stock;
        await _productRepository.UpdateAsync(product, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindAsync(id, cancellationToken);
        if (!IsUserOwner(product))
            throw new UnauthorizedAccessException("Ação não permitida.");

        await _productRepository.DeleteAsync(id, cancellationToken);
    }

    private bool IsUserOwner(Product? product)
    {
        return product != null && product.SellerId == _currentUserId;
    }

    private Guid GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
            return Guid.Empty;

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new Exception("Usuário não encontrado");

        return Guid.Parse(userId);
    }
}

public interface IProductService
{
    Task<List<Product>> GetAsync(CancellationToken cancellationToken);

    Task<List<Product>> GetBySellerId(CancellationToken cancellationToken);

    Task<Product> FindAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken);

    Task<Guid> CreateAsync(CreateProductViewModel createProductViewModel, CancellationToken cancellationToken,
        string? path = null);

    Task UpdateAsync(UpdateProductViewModel updateProductViewModel, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}