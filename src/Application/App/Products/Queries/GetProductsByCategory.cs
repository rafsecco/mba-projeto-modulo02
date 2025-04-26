using Core.Data;
using Core.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.App.Products.Queries;

public class GetProductsByCategory : IRequest<List<Product>>
{
    public int CategoryId { get; set; }
}

public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategory, List<Product>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetProductsByCategoryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> Handle(GetProductsByCategory request, CancellationToken cancellationToken)
    {
        var products = await _dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Seller).Where(p => p.CategoryId == request.CategoryId && !p.Deleted)
            .ToListAsync(cancellationToken);
        return products;
    }
}

public class GetProductsByCategoryValidator : AbstractValidator<GetProductsByCategory>
{
    public GetProductsByCategoryValidator()
    {
        RuleFor(p => p.CategoryId)
            .GreaterThan(0);
    }
}