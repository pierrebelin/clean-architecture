using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Persistence;
using MassTransit;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Core.Customers.Commands;

public sealed class DeleteCustomerCommandConsumer : IConsumer<DeleteCustomerCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerCommandConsumer(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
    }

    public async Task Consume(ConsumeContext<DeleteCustomerCommand> context)
    {
        var customer = await _customerRepository.GetByIdAsync(context.Message.Id);
        if (customer is null)
        {
            await context.RespondAsync<Result<bool, IDbResult>>(new NotSaved());
            return;
        }

        _customerRepository.Remove(customer);
        var result = await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        if (result == 0)
        {
            await context.RespondAsync<Result<bool, IDbResult>>(new NotSaved());
            return;
        }
        await context.RespondAsync<Result<bool, IDbResult>>(true);
    }
}
