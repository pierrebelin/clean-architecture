using CleanArchitecture.Application.Mediator;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Core.Customers.Commands;

public record DeleteCustomerCommand(Guid Id);
