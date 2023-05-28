using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Persistence;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Core.Customers.Commands;

public sealed class UpdateCustomerCommandHandler : MediatorRequestHandler<UpdateCustomerCommand, Result<bool, IDbResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task<Result<bool, IDbResult>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.Id);
        if (customer is null)
        {
            return new NotFound();
        }

        customer.Name = request.Name;
        _unitOfWork.CustomerRepository.Update(customer);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            return new NotSaved();
        }
        return true;
    }
}
