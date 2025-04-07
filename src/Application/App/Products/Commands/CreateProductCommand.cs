using Application.Data;
using Application.Domain.Entities;
using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.App.Products.Commands;

public class CreateProductCommand : IRequest<int>
{
    public int CategoryId { get; set; }

    [JsonIgnore]
    public Guid SellerId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    //TODO: verificar como fazer o upload da imagem
    public string ImageUrl { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateProductCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
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
            Image = request.ImageUrl
        };

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
        RuleFor(p => p.ImageUrl)
            .NotEmpty();
        RuleFor(p => p.CategoryId)
            .GreaterThan(0);
        //RuleFor(p => p.SellerId)
        //    .NotEmpty();
    }
}