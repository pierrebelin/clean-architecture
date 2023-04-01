using CleanArchitecture.Domain.DomainObjects;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Customers.Commands;

public record CreateCustomerCommand(string Name) : Request<Result<bool>>;
