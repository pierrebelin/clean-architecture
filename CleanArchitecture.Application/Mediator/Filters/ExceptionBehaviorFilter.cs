using CleanArchitecture.Application.Mediator.Filters.Utils;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArchitecture.Application.Mediator.Filters;

public class ExceptionBehaviorFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    private readonly ILogger<ExceptionBehaviorFilter<T>> _logger;

    public ExceptionBehaviorFilter(ILogger<ExceptionBehaviorFilter<T>> logger)
    {
        _logger = logger;
    }
    
    public void Probe(ProbeContext context)
    {
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    { 
        try
        {
            await next.Send(context);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            await context.RespondWithError(new NotFound());
        }
    }
}