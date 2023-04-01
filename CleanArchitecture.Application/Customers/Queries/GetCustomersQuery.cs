using CleanArchitecture.Domain.DomainObjects;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Customers.Queries;

public record GetCustomersQuery() : Request<Result<IEnumerable<Customer>>>;