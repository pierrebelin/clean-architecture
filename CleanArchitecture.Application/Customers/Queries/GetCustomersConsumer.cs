using CleanArchitecture.Application.Products.Queries;
using CleanArchitecture.Domain.DomainObjects;
using CleanArchitecture.Domain.Persistence;
using MassTransit;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Customers.Queries;

public sealed class GetCustomersConsumer : MediatorRequestHandler<GetCustomersQuery, Result<IEnumerable<Customer>>>
{
    private readonly IDataServiceFactory _dataServiceFactory;

    public GetCustomersConsumer(IDataServiceFactory dataServiceFactory)
    {
        _dataServiceFactory = dataServiceFactory;
    }

    protected override async Task<Result<IEnumerable<Customer>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var dataService = _dataServiceFactory.CreateService<Customer>();
        var customers = await dataService.GetAllAsync();
        return new Result<IEnumerable<Customer>>() { Value = customers };
    }
}


