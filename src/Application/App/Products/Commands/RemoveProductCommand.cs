using Application.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.App.Products.Commands;

public class RemoveProductCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommand, Unit>
{
    private readonly ApplicationDbContext _dbContext;

    public RemoveProductCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id && !p.Deleted, cancellationToken);
        if (product is not null)
        {
            product.Deleted = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}

public class RemoveProductCommandValidator : AbstractValidator<RemoveProductCommand>
{
    public RemoveProductCommandValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0);
    }
}