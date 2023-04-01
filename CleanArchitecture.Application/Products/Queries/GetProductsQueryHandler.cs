using CleanArchitecture.Domain.DomainObjects;
using CleanArchitecture.Domain.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Products.Queries;

internal sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<IEnumerable<Product>>>
{
    private readonly IDataServiceFactory _dataServiceFactory;

    public GetProductsQueryHandler(IDataServiceFactory dataServiceFactory)
    {
        _dataServiceFactory = dataServiceFactory;
    }

    public async Task<Result<IEnumerable<Product>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var dataService = _dataServiceFactory.CreateService<Product>();
        var products = await dataService.GetAllAsync();
        return new Result<IEnumerable<Product>>() {Value = products};
    }
}


