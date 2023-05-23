using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.UseCases.Customers.Queries;

public record GetCustomerQuery(Guid Id) : Request<Result<Customer, NotFound>>;