using Core.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.App.Categories.Commands;

public class RemoveCategoryCommand : IRequest
{
    public int Id { get; set; }
}

public class RemoveCategoryCommandHandler : IRequestHandler<RemoveCategoryCommand>
{
    private readonly ApplicationDbContext _dbContext;

    public RemoveCategoryCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == request.Id && !c.Deleted, cancellationToken);
        if (category is not null)
        {
            category.Deleted = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

public class RemoveCategoryCommandValidator : AbstractValidator<RemoveCategoryCommand>
{
    //TODO: nao permitir a exclusao de categorias com produtos
    public RemoveCategoryCommandValidator()
    {
        RuleFor(c => c.Id)
            .GreaterThan(0);
    }
}