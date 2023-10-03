using MassTransit.Mediator;

namespace CleanArchitecture.Application.Mediator.Dispatcher;

public interface IRequest<TOutput> : Request<Result<TOutput, IErrorReason>> where TOutput : class
{
}

public interface IQuery<TOutput> : IRequest<TOutput> where TOutput : class
{
}