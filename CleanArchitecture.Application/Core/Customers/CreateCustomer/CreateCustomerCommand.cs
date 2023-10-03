using STID.SMID.Application.Core.Dispatcher;

namespace CleanArchitecture.Application.Core.Customers.CreateCustomer;

public record CreateCustomerCommand(string Name) : ICommand<CreateCustomerResult>;
