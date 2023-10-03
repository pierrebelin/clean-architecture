using MassTransit;
using STID.SMID.Application.Core.Dispatcher;

namespace CleanArchitecture.Application.Mediator.Dispatcher;

public abstract class CommandHandler<TCommand, TOutput> : IConsumer<TCommand>
    where TCommand : class, ICommand<TOutput>
    where TOutput : class
{

    public async Task Consume(ConsumeContext<TCommand> context)
    {
        var result = await Handle(context.Message, context.CancellationToken);
        await context.RespondAsync(result);
    }

    protected abstract Task<Result<TOutput, IErrorReason>> Handle(TCommand request, CancellationToken cancellationToken);
}