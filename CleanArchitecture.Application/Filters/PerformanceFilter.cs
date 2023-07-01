using System.Diagnostics;
using MassTransit;
using MediatR;

namespace CleanArchitecture.Application.Filters
{
    public class PerformanceFilter<T> : IFilter<ConsumeContext<T>> where T : class
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<T> _logger;

        public PerformanceFilter(ILogger<T> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            _timer.Start();
            await next.Send(context);
            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;
            if (elapsedMilliseconds > 500)
            {
                _logger.LogWarning($"CleanArchitecture Long Running Request: {typeof(T).Name} ({elapsedMilliseconds} milliseconds)");
            }
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("performance");
        }
    }
}