using MassTransit;

namespace CleanArchitecture.Application.Mediator.Dispatcher;

public abstract class QueryHandler<TQuery,TOutput> : IConsumer<TQuery>
    where TQuery : class, IQuery<TOutput> 
    where TOutput : class
{

    public async Task Consume(ConsumeContext<TQuery> context)
    {
        var result = await Handle(context.Message, context.CancellationToken);
        await context.RespondAsync(result);
    }

    protected abstract Task<Result<TOutput, IErrorReason>> Handle(TQuery request, CancellationToken cancellationToken);
}