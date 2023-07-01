using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MediatR;

namespace CleanArchitecture.Application.Core.Products.Commands;

internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<bool, ValidationFailed>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<Result<bool, ValidationFailed>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product() { Name = request.Name };
        _productRepository.Add(product);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            return new ValidationFailed();
        }
        return true;
    }
}

