namespace CleanArchitecture.Application.Mediator.Dispatcher;

public interface IDispatcher
{
    Task<Result<TOutput, IErrorReason>> Send<TOutput>(IRequest<TOutput> request, CancellationToken cancellationToken = default) where TOutput : class;
}