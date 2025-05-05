using Core.Data.Repositories;
using Core.Domain.Entities;
using Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private IHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Guid _currentUserId;

    public ProductService(IProductRepository productRepository, IHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _productRepository = productRepository;
        _hostingEnvironment = hostingEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _currentUserId = GetCurrentUserId();
    }

    public async Task<List<Product>> GetAsync(CancellationToken cancellationToken)
    {
        return await _productRepository.GetBySellerIdAsync(_currentUserId, cancellationToken);
    }

    public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        return IsUserOwner(product) ? product : null;
    }

    public async Task<List<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _productRepository.GetByCategoryIdAsync(categoryId, cancellationToken);
    }

    public async Task<Guid> CreateAsync(CreateProductViewModel createProductViewModel, CancellationToken cancellationToken, string? path = null)
    {
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
        {
            await AddImage(product, createProductViewModel.UploadImage, path, cancellationToken);
        }

        return await _productRepository.CreateAsync(product, cancellationToken);
    }

    public async Task UpdateAsync(UpdateProductViewModel updateProductViewModel, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(updateProductViewModel.Id, cancellationToken);
        if (product is null) throw new Exception("Produto não encontrado");

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
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (!IsUserOwner(product))
            throw new UnauthorizedAccessException("Ação não permitida.");

        await _productRepository.DeleteAsync(id, cancellationToken);
    }

    private async Task AddImage(Product product, IFormFile uploadImage, string? path, CancellationToken cancellationToken)
    {
        const string imagesFolder = "images";
        var fullPath = !string.IsNullOrEmpty(path) ? path + "/" + imagesFolder : imagesFolder;
        var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, fullPath);
        var exists = Directory.Exists(uploads);

        if (!exists)
            Directory.CreateDirectory(uploads);
        var extension = uploadImage.FileName.Split('.');
        var filename = $"{Guid.NewGuid()}.{extension[1]}";
        var filePath = Path.Combine(uploads, filename);
        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await uploadImage.CopyToAsync(fileStream, cancellationToken);
            product.Image = filename;
        }
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
    public Task<List<Product>> GetAsync(CancellationToken cancellationToken);

    public Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<List<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken);

    public Task<Guid> CreateAsync(CreateProductViewModel createProductViewModel, CancellationToken cancellationToken, string? path = null);

    public Task UpdateAsync(UpdateProductViewModel updateProductViewModel, CancellationToken cancellationToken);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}