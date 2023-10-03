using CleanArchitecture.Application.Mediator.Dispatcher;

namespace CleanArchitecture.Application.Core.Customers.GetCustomer;

public record GetCustomerQuery(Guid Id) : IQuery<GetCustomerResult>;