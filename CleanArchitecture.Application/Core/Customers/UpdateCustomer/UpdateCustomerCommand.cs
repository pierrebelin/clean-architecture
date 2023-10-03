using STID.SMID.Application.Core.Dispatcher;

namespace CleanArchitecture.Application.Core.Customers.UpdateCustomer;

public record UpdateCustomerCommand(Guid Id, string Name) : ICommand<UpdateCustomerResult>;
