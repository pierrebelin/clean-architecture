using CleanArchitecture.Application.Mediator;
using FluentValidation;
using MassTransit;

namespace CleanArchitecture.Application.Core.Customers.CreateCustomer
{
    public class CreateCustomerValidator : IFilter<ConsumeContext<CreateCustomerCommand>>
    {
        public async Task Send(ConsumeContext<CreateCustomerCommand> context, IPipe<ConsumeContext<CreateCustomerCommand>> next)
        {
            var validator = new CreateCustomerCommandValidator();
            if (!(await validator.ValidateAsync(context.Message)).IsValid)
            {
                await context.RespondAsync<Result<bool, WrongParameters>>(new WrongParameters());
                await context.NotifyConsumed(TimeSpan.Zero, "addCustomerValidatorFilter");
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