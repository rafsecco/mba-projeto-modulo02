using Application.Data;
using Application.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace Application.App.Products.Commands;

public class CreateProductCommand : IRequest<int>
{
    public int CategoryId { get; set; }

    [JsonIgnore]
    public Guid SellerId { get; set; }

    public IFormFile UploadImage { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly ApplicationDbContext _dbContext;
    private IHostEnvironment _hostingEnvironment;

    public CreateProductCommandHandler(ApplicationDbContext dbContext, IHostEnvironment hostingEnvironment)
    {
        _dbContext = dbContext;
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            CategoryId = request.CategoryId,
            SellerId = request.SellerId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.StockQuantity,
        };

        var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");
        var exists = Directory.Exists(uploads);

        if (!exists)
            Directory.CreateDirectory(uploads);

        if (request.UploadImage != null)
        {
            var extension = request.UploadImage.FileName.Split('.');
            var filename = $"{Guid.NewGuid()}.{extension[1]}";
            var filePath = Path.Combine(uploads, filename);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.UploadImage.CopyToAsync(fileStream, cancellationToken);
                product.Image = filename;
            }
        }

        await _dbContext.Products.AddAsync(product, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return product.Id;
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(300);
        RuleFor(p => p.Price)
            .GreaterThan(0);
        RuleFor(p => p.StockQuantity)
            .GreaterThan(0);
        RuleFor(p => p.CategoryId)
            .GreaterThan(0);
    }
}