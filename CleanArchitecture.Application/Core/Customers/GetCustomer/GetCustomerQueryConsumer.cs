using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Application.Mediator.Dispatcher;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MassTransit;

namespace CleanArchitecture.Application.Core.Customers.GetCustomer;

public sealed class GetCustomerQueryConsumer : QueryHandler<GetCustomerQuery, GetCustomerResult>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerQueryConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    protected override async Task<Result<GetCustomerResult, IErrorReason>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            return new NotFound();
        }
        return customer.ConvertToGetCustomerResult();
    }
}


