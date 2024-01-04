using CleanArchitecture.Application.Core.Customers.DeleteCustomer;
using MassTransit;

namespace CleanArchitecture.Application.Core.Historization.Consumer
{
    public class AddHistorizationConsumer : IConsumer<AddHistorizationCommand>
    {
        private readonly ILogger<AddHistorizationConsumer> _logger;

        public AddHistorizationConsumer(ILogger<AddHistorizationConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<AddHistorizationCommand> context)
        {
            _logger.LogInformation(context.Message.ObjectToHistorize.ToString());
            await context.Publish(new AddHistorizationSucceedEvent(context.Message.Id));
        }
    }
}