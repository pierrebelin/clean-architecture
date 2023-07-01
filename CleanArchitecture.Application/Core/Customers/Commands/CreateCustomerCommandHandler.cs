using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Core.Customers.Commands;

public sealed class CreateCustomerCommandHandler : MediatorRequestHandler<CreateCustomerCommand, Result<bool, ValidationFailed>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
    }

    protected override async Task<Result<bool, ValidationFailed>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer() { Name = request.Name };

        _customerRepository.Add(customer);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            return new ValidationFailed();
        }
        return true;
    }
}
