using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Persistence;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Core.Customers.Commands;

public sealed class DeleteCustomerCommandHandler : MediatorRequestHandler<DeleteCustomerCommand, Result<bool, IDbResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
    }

    protected override async Task<Result<bool, IDbResult>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer is null)
        {
            return new NotFound();
        }

        _customerRepository.Remove(customer);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            return new NotSaved();
        }
        return true;
    }
}
