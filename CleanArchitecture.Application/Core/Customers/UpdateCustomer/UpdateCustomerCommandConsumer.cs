using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Application.Mediator.Dispatcher;
using CleanArchitecture.Domain.Persistence;
using MassTransit;

namespace CleanArchitecture.Application.Core.Customers.UpdateCustomer;

public sealed class UpdateCustomerCommandConsumer : CommandHandler<UpdateCustomerCommand, UpdateCustomerResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandConsumer(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
    }


    protected override async Task<Result<UpdateCustomerResult, IErrorReason>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer is null)
        {
            return new NotFound();
        }

        customer.Name = request.Name;
        _customerRepository.Update(customer);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            return new NotSaved();
        }

        return new UpdateCustomerResult(true);
    }
}
