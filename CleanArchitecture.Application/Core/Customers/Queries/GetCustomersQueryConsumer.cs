using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MassTransit;

namespace CleanArchitecture.Application.Core.Customers.Queries;

public sealed class GetCustomersQueryConsumer : IConsumer<GetCustomersQuery>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersQueryConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Consume(ConsumeContext<GetCustomersQuery> context)
    {
        var customers = await _customerRepository.GetAllAsync();
        if (!customers.Any())
        {
            await context.RespondAsync<Result<IEnumerable<Customer>, NotFound>>(new NotFound());
        }
        await context.RespondAsync<Result<IEnumerable<Customer>, NotFound>>(customers);
    }
}