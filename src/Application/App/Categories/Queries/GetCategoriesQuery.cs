using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Data;
using Application.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.App.Categories.Queries;

public class GetCategoriesQuery : IRequest<List<Category>>
{
}

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<Category>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetCategoriesQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _dbContext.Categories
            .Where(c => !c.Deleted)
            .ToListAsync(cancellationToken);
        return categories;
    }
}