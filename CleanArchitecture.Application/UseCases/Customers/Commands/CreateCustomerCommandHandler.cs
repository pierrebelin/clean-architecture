using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.UseCases.Customers.Commands;

public sealed class CreateCustomerCommandHandler : MediatorRequestHandler<CreateCustomerCommand, Result<bool, ValidationFailed>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task<Result<bool, ValidationFailed>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer() { Name = request.Name };

        _unitOfWork.CustomerRepository.Add(customer);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            return new ValidationFailed();
        }
        return true;
    }
}
