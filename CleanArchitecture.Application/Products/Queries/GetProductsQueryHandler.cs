using CleanArchitecture.Domain.DomainObjects;
using CleanArchitecture.Domain.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Products.Queries;

internal sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
    private readonly IDataServiceFactory _dataServiceFactory;

    public GetProductsQueryHandler(IDataServiceFactory dataServiceFactory)
    {
        _dataServiceFactory = dataServiceFactory;
    }

    public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var dataService = _dataServiceFactory.CreateService<Product>();
        var products = await dataService.GetAllAsync();
        return products;
    }
}


