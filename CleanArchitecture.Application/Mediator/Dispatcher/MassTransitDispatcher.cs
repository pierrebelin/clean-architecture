using MassTransit;
using MassTransit.Mediator;
using STID.SMID.Application.Core.Dispatcher;

namespace CleanArchitecture.Application.Mediator.Dispatcher;

public class MassTransitDispatcher : IDispatcher
{
    private readonly IMediator _mediator;

    public MassTransitDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<Result<TOutput, IErrorReason>> Send<TOutput>(IRequest<TOutput> request, CancellationToken cancellationToken = default) where TOutput : class
    {
        return _mediator.SendRequest(request, cancellationToken);
    }
}