using CleanArchitecture.Application.Core.Customers.Commands;
using CleanArchitecture.Application.Core.Customers.Queries;
using CleanArchitecture.Application.Mediator;
using FluentValidation;
using MassTransit;
using MassTransit.Configuration;

namespace CleanArchitecture.Application.Core.Validators
{
    public class AddCustomerValidatorFilter : IFilter<ConsumeContext<CreateCustomerCommand>>
    {
        public async Task Send(ConsumeContext<CreateCustomerCommand> context, IPipe<ConsumeContext<CreateCustomerCommand>> next)
        {
            var validator = new CreateCustomerCommandValidator();
            if (!(await validator.ValidateAsync(context.Message)).IsValid)
            {
                await context.NotifyConsumed(TimeSpan.Zero, "addCustomerValidatorFilter");
                await context.RespondAsync<Result<bool, ValidationFailed>>(new ValidationFailed());
                return;
            }
            await next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
        }
    }

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(_ => _.Name).MinimumLength(3).WithMessage("Name must be more than 2 characters");
        }
    }
}