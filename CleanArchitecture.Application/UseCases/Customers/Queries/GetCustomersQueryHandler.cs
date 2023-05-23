using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MassTransit.Mediator;

namespace CleanArchitecture.Application.UseCases.Customers.Queries;

public sealed class GetCustomersQueryHandler : MediatorRequestHandler<GetCustomersQuery, Result<IEnumerable<Customer>, NotFound>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task<Result<IEnumerable<Customer>, NotFound>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _unitOfWork.CustomerRepository.GetAllAsync();
        if (!customers.Any())
        {
            return new NotFound();
        }
        return customers;
    }
}


