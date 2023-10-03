using CleanArchitecture.Application.Mediator.Dispatcher;

namespace CleanArchitecture.Application.Core.Customers.GetCustomers;

public record GetCustomersQuery() : IQuery<GetCustomerResults>;