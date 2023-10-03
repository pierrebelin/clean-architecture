using CleanArchitecture.Application.Core.Customers.GetCustomer;

namespace CleanArchitecture.Application.Core.Customers.GetCustomers;

public record GetCustomerResults(IEnumerable<GetCustomerResult> Customers);