using CleanArchitecture.Domain.DomainObjects;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Customers.Queries;

public record GetCustomerQuery(Guid Id) : Request<Result<Customer>>;