using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MediatR;

namespace CleanArchitecture.Application.UseCases.Products.Queries;

internal sealed class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<Product, NotFound>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Product, NotFound>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            return new NotFound();
        }
        return product;
    }
}


