namespace CleanArchitecture.Application.Core.Customers.DeleteCustomer;

public record DeleteCustomerCommand
{
    public Guid Id { get; init; }
}
