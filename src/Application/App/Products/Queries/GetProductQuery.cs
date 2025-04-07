using Application.Data;
using Application.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.App.Products.Queries;

public class GetProductQuery : IRequest<Product>
{
    public int Id { get; set; }
}

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
{
    private readonly ApplicationDbContext _dbContext;

    public GetProductQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == request.Id && !p.Deleted, cancellationToken);

        return product;
    }
}

public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0);
    }
}