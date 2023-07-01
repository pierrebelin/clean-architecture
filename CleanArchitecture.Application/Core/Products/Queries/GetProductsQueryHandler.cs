using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MediatR;

namespace CleanArchitecture.Application.Core.Products.Queries;

internal sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<IEnumerable<Product>, NotFound>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<IEnumerable<Product>, NotFound>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var customers = await _productRepository.GetAllAsync();
        if (!customers.Any())
        {
            return new NotFound();
        }
        return customers;
    }
}


