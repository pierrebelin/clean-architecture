using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.DomainObjects;
using CleanArchitecture.Domain.Persistence;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.UseCases.Customers.Commands;

public sealed class CreateCustomerConsumer : MediatorRequestHandler<CreateCustomerCommand, Result<bool>>
{
    private readonly IDataServiceFactory _dataServiceFactory;

    public CreateCustomerConsumer(IDataServiceFactory dataServiceFactory)
    {
        _dataServiceFactory = dataServiceFactory;
    }

    protected override async Task<Result<bool>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer() { Name = request.Name };

        IDataService<Customer> dataService = _dataServiceFactory.CreateService<Customer>();
        dataService.Add(customer);

        var result = await _dataServiceFactory.SaveChangesAsync(cancellationToken);
        return new Result<bool>() { Value = result > 0 };
    }
}


