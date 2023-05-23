using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MediatR;

namespace CleanArchitecture.Application.UseCases.Products.Queries;

internal sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<IEnumerable<Product>, NotFound>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<Product>, NotFound>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var customers = await _unitOfWork.ProductRepository.GetAllAsync();
        if (!customers.Any())
        {
            return new NotFound();
        }
        return customers;
    }
}


