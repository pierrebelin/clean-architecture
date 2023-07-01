using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Persistence;
using MassTransit;
using MassTransit.Mediator;
using static MassTransit.MessageHeaders;
using System.Threading;

namespace CleanArchitecture.Application.Core.Customers.Commands;

public sealed class UpdateCustomerCommandConsumer : IConsumer<UpdateCustomerCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandConsumer(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
    }

    public async Task Consume(ConsumeContext<UpdateCustomerCommand> context)
    {
        var customer = await _customerRepository.GetByIdAsync(context.Message.Id);
        if (customer is null)
        {
            await context.RespondAsync<Result<bool, ValidationFailed>>(new NotFound());
        }

        customer.Name = context.Message.Name;
        _customerRepository.Update(customer);
        var result = await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        if (result == 0)
        {
            await context.RespondAsync<Result<bool, ValidationFailed>>(new NotSaved());
        }
        await context.RespondAsync<Result<bool, ValidationFailed>>(true);
    }
}
