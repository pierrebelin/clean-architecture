using CleanArchitecture.Application.Core.Customers.GetCustomer;
using CleanArchitecture.Application.Core.Customers.GetCustomers;
using CleanArchitecture.Domain.Entities;
using MassTransit.Futures.Contracts;

namespace CleanArchitecture.Application.Core.Customers;
public static class CustomersMapper
{
    public static GetCustomerResult ConvertToGetCustomerResult(this Customer customer)
    {
        return new GetCustomerResult(customer.Id, customer.Name);
    }
    
    public static GetCustomerResults ConvertToGetCustomerResult(this IEnumerable<Customer> customers)
    {
        var results = customers.Select(c => c.ConvertToGetCustomerResult());
        return new GetCustomerResults(results);
    }
}
