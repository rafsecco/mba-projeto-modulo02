using Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.App.Products.Commands;

public class UpdateProductCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? StockQuantity { get; set; }
    public int? CategoryId { get; set; }
    public Guid? SellerId { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateProductCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id && !p.Deleted, cancellationToken);

        if (product is not null)
        {
            product.Name = request.Name ?? product.Name;
            product.Description = request.Description ?? product.Description;
            product.Price = request.Price ?? product.Price;
            product.Stock = request.StockQuantity ?? product.Stock;
            product.CategoryId = request.CategoryId ?? product.CategoryId;
            product.SellerId = request.SellerId ?? product.SellerId;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}