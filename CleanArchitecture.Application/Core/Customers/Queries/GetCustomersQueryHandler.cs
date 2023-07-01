using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Core.Customers.Queries;

public sealed class GetCustomersQueryHandler : MediatorRequestHandler<GetCustomersQuery, Result<IEnumerable<Customer>, NotFound>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    protected override async Task<Result<IEnumerable<Customer>, NotFound>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.GetAllAsync();
        if (!customers.Any())
        {
            return new NotFound();
        }
        return customers;
    }
}


