using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Core.Customers.Queries;

public record GetCustomersQuery() : Request<Result<IEnumerable<Customer>, NotFound>>;