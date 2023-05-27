using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.Core.Customers.Queries;

public sealed class GetCustomerQueryHandler : MediatorRequestHandler<GetCustomerQuery, Result<Customer, NotFound>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task<Result<Customer, NotFound>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            return new NotFound();
        }
        return customer;
    }
}


