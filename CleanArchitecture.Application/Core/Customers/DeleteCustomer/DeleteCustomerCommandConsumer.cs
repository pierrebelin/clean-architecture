using CleanArchitecture.Application.Core.Historization.Consumer;
using CleanArchitecture.Domain.Persistence;
using MassTransit;

namespace CleanArchitecture.Application.Core.Customers.DeleteCustomer;

public class CustomerDeletionSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public State CurrentState { get; set; }
}

public class CustomerDeletionSagaStateDefinition : SagaDefinition<CustomerDeletionSagaState>
{

}

public class CustomerDeletionStateMachine : MassTransitStateMachine<CustomerDeletionSagaState>
{
    public CustomerDeletionStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => CustomerDeleted, x => x.CorrelateById(context => context.Message.Id));
        Event(() => HistorizationAdded, x => x.CorrelateById(context => context.Message.Id));
        Event(() => StartCustomerDeletionEvent, x => x.CorrelateById(context => context.Message.Id));

        SetCompletedWhenFinalized();

        Initially(
            When(StartCustomerDeletionEvent)
                .PublishAsync(_ => _.Init<DeleteCustomerCommand>(new DeleteCustomerCommand() {Id = _.Message.Id }))
                .TransitionTo(DeleteCustomer)
            );

        During(DeleteCustomer,
            When(CustomerDeleted)
                .PublishAsync(_ => _.Init<AddHistorizationCommand>(new AddHistorizationCommand(_.Message.Id, _.Message)))
                .TransitionTo(AddHistorization)
        );

        During(AddHistorization,
            When(HistorizationAdded)
                .Finalize()
        );
    }

    public Event<StartCustomerDeletionEvent> StartCustomerDeletionEvent { get; set; }
    public Event<CustomerDeletedEvent> CustomerDeleted { get; set; }
    public Event<AddHistorizationSucceedEvent> HistorizationAdded { get; set; }

    public State DeleteCustomer { get; }
    public State AddHistorization { get; }
}

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
            await context.Publish<CustomerDeleteFailedEvent>(new { context.Message.Id });
            return;
        }

        //_customerRepository.Remove(customer);
        //var result = await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        //if (result == 0)
        //{
        //    await context.Publish<CustomerDeleteFailedEvent>(new { context.Message.Id });
        //    return;
        //}
        await context.Publish(new CustomerDeletedEvent(context.Message.Id));
        //await context.RespondAsync(new CustomerDeletedEvent(context.Message.Id));
    }
}

public record StartCustomerDeletionEvent(Guid Id);
public record CustomerDeletedEvent(Guid Id);
public record AddHistorizationSucceedEvent(Guid Id);
public record CustomerDeleteFailedEvent(Guid Id);
