using Core.Data.Repositories;
using Core.Domain.Entities;
using Microsoft.Extensions.Hosting;

namespace Core.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private IHostEnvironment _hostingEnvironment;

    public ProductService(IProductRepository productRepository, IHostEnvironment hostingEnvironment)
    {
        _productRepository = productRepository;
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<List<Product>> GetAsync(CancellationToken cancellationToken)
    {
        return await _productRepository.GetAsync(cancellationToken);
    }

    public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _productRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<List<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _productRepository.GetByCategoryIdAsync(categoryId, cancellationToken);
    }

    public async Task<Guid> CreateAsync(Product product, CancellationToken cancellationToken)
    {
        if (product.UploadImage != null)
        {
            await AddImage(product, cancellationToken);
        }

        return await _productRepository.CreateAsync(product, cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        await _productRepository.UpdateAsync(product, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _productRepository.DeleteAsync(id, cancellationToken);
    }

    private async Task AddImage(Product product, CancellationToken cancellationToken)
    {
        var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");
        var exists = Directory.Exists(uploads);

        if (!exists)
            Directory.CreateDirectory(uploads);
        var extension = product.UploadImage.FileName.Split('.');
        var filename = $"{Guid.NewGuid()}.{extension[1]}";
        var filePath = Path.Combine(uploads, filename);
        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await product.UploadImage.CopyToAsync(fileStream, cancellationToken);
            product.Image = filename;
        }
    }
}

public interface IProductService
{
    public Task<List<Product>> GetAsync(CancellationToken cancellationToken);

    public Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<List<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken);

    public Task<Guid> CreateAsync(Product product, CancellationToken cancellationToken);

    public Task UpdateAsync(Product product, CancellationToken cancellationToken);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}