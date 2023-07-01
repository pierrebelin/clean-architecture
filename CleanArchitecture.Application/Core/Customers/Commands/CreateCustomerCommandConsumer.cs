using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MassTransit;
using System.Threading;

namespace CleanArchitecture.Application.Core.Customers.Commands;

public sealed class CreateCustomerCommandConsumer : IConsumer<CreateCustomerCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandConsumer(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
    }

    public async Task Consume(ConsumeContext<CreateCustomerCommand> context)
    {
        var customer = new Customer() { Name = context.Message.Name };

        _customerRepository.Add(customer);
        var result = await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        if (result == 0)
        {
            await context.RespondAsync<Result<bool, ValidationFailed>>(new ValidationFailed());
        }
        await context.RespondAsync<Result<bool, ValidationFailed>>(true);
    }
}
