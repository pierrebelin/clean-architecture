using CleanArchitecture.Domain.Persistence;
using MassTransit;

namespace CleanArchitecture.Application.Core.Customers.GetCustomers;

public sealed class GetCustomersQueryConsumer : IConsumer<GetCustomersQuery>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersQueryConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task Consume(ConsumeContext<GetCustomersQuery> context)
    {
        Console.WriteLine(DateTime.Now);
        return Task.Delay(10000);
        // await Task.Delay(10000);
        // return;
    }
}

// public sealed class GetCustomersQueryConsumerv2 : QueryHandler<GetCustomersQuery, GetCustomerResults>
public sealed class GetCustomersQueryConsumerv2 : IConsumer<GetCustomersQuery>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersQueryConsumerv2(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    public Task Consume(ConsumeContext<GetCustomersQuery> context)
    {
        // Thread.Sleep(10000);
        Console.WriteLine(DateTime.Now);
        return Task.Delay(10000);
        // await Task.Delay(10000);
        // return;
    }
}