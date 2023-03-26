using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Products.Queries;

internal sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
    private readonly IDapperDbContext _dbContext;

    public GetProductsQueryHandler(IDapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = _dbContext.Query<Product>(@$"SELECT {nameof(Product.Id)}, {nameof(Product.Name)} FROM Products");
        return products;
    }
}


