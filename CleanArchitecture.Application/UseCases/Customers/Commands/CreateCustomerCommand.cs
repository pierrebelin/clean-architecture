using CleanArchitecture.Application.Mediator;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.UseCases.Customers.Commands;

public record CreateCustomerCommand(string Name) : Request<Result<bool, ValidationFailed>>;
