using Core.Data;
using Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Core.App.Categories.Commands;

public class CreateCategoryCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateCategoryCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };
        await _dbContext.Categories.AddAsync(category, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return category.Id;
    }
}

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(300);
    }
}