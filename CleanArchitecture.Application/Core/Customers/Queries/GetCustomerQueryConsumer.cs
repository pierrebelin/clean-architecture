using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MassTransit;
using MassTransit.Mediator;
using static MassTransit.MessageHeaders;

namespace CleanArchitecture.Application.Core.Customers.Queries;

public sealed class GetCustomerQueryConsumer : IConsumer<GetCustomerQuery>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerQueryConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Consume(ConsumeContext<GetCustomerQuery> context)
    {
        var customer = await _customerRepository.GetByIdAsync(context.Message.Id);
        if (customer == null)
        {
            await context.RespondAsync<Result<Customer, NotFound>>(new NotFound());
            return;
        }
        await context.RespondAsync<Result<Customer, NotFound>>(customer);
    }
}