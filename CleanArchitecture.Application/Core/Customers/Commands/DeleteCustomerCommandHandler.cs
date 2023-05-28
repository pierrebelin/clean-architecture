using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Persistence;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Core.Customers.Commands;

public sealed class DeleteCustomerCommandHandler : MediatorRequestHandler<DeleteCustomerCommand, Result<bool, IDbResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task<Result<bool, IDbResult>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.Id);
        if (customer is null)
        {
            return new NotFound();
        }

        _unitOfWork.CustomerRepository.Remove(customer);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            return new NotSaved();
        }
        return true;
    }
}
