using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data;
using Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.App.Products.Queries
{
    public class GetProductsQuery : IRequest<List<Product>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<Product>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetProductsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .Where(p => !p.Deleted)
                .ToListAsync(cancellationToken);
            return products;
        }
    }
}