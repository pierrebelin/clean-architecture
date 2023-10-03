using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Application.Mediator.Dispatcher;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MassTransit;

namespace CleanArchitecture.Application.Core.Customers.CreateCustomer;

public sealed class CreateCustomerCommandConsumer : CommandHandler<CreateCustomerCommand, CreateCustomerResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandConsumer(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
    }

    protected override async Task<Result<CreateCustomerResult, IErrorReason>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer() { Name = request.Name };

        _customerRepository.Add(customer);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            return new NotSaved();
        }
        
        return new CreateCustomerResult(true);
    }
}
