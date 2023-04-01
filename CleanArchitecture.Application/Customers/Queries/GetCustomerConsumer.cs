using CleanArchitecture.Application.Products.Queries;
using CleanArchitecture.Domain.DomainObjects;
using CleanArchitecture.Domain.Persistence;
using MassTransit;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Customers.Queries;

public sealed class GetCustomerConsumer : MediatorRequestHandler<GetCustomerQuery, Result<Customer>>
{
    private readonly IDataServiceFactory _dataServiceFactory;

    public GetCustomerConsumer(IDataServiceFactory dataServiceFactory)
    {
        _dataServiceFactory = dataServiceFactory;
    }

    protected override async Task<Result<Customer>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var dataService = _dataServiceFactory.CreateService<Customer>();
        var customer = dataService.GetFirstOfDefault(_ => _.Id == request.Id);
        return new Result<Customer>() { Value = customer };
    }
}


