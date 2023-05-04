using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.DomainObjects;
using CleanArchitecture.Domain.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.UseCases.Products.Queries;

internal sealed class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<Product>>
{
    //private readonly IDapperDbContext _dbContext;

    //public GetProductQueryHandler(IDapperDbContext dbContext)
    //{
    //    _dbContext = dbContext;
    //}

    private readonly IDataServiceFactory _dataServiceFactory;
    public GetProductQueryHandler(IDataServiceFactory dataServiceFactory)
    {
        _dataServiceFactory = dataServiceFactory;
    }

    public async Task<Result<Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var dataService = _dataServiceFactory.CreateService<Product>();
        var product = dataService.GetFirstOfDefault(_ => _.Id == request.Id);
        return new Result<Product>() { Value = product };
        //Product product = await _dbContext.QueryFirstOrDefaultAsync<Product>(
        //    @$"SELECT {nameof(Product.Id)}, {nameof(Product.Name)} 
        //            FROM Products
        //            WHERE {nameof(Product.Id)} = @Id",
        //        new { Id = request.Id.ToString().ToUpper() });
        //return product;
    }
}


