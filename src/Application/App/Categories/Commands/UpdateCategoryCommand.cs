using Application.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.App.Categories.Commands;

public class UpdateCategoryCommand : IRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Id { get; set; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateCategoryCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == request.Id && !c.Deleted, cancellationToken);
        if (category is not null)
        {
            category.Name = request.Name ?? category.Name;
            category.Description = request.Description ?? category.Description;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Id)
            .GreaterThan(0);
        RuleFor(c => c.Name)
            .MaximumLength(100);
        RuleFor(c => c.Description)
            .MaximumLength(300);
    }
}