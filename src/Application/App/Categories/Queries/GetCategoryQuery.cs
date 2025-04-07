using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Data;
using Application.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.App.Categories.Queries;

public class GetCategoryQuery : IRequest<Category>
{
    public int Id { get; set; }
}

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Category>
{
    private readonly ApplicationDbContext _dbContext;

    public GetCategoryQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Category> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id && !c.Deleted, cancellationToken);
        return category;
    }
}

public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
{
    public GetCategoryQueryValidator()
    {
        RuleFor(c => c.Id)
            .GreaterThan(0);
    }
}