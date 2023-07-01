using CleanArchitecture.Application.Mediator;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Core.Customers.Commands;

public record UpdateCustomerCommand(Guid Id, string Name);
