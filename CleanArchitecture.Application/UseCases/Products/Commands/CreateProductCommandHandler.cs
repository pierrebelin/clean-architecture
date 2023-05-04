using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.DomainObjects;
using CleanArchitecture.Domain.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using static MassTransit.ValidationResultExtensions;

namespace CleanArchitecture.Application.UseCases.Products.Commands;

internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<bool>>
{
    private readonly IDataServiceFactory _dataServiceFactory;

    public CreateProductCommandHandler(IDataServiceFactory dataServiceFactory)
    {
        _dataServiceFactory = dataServiceFactory;
    }

    public async Task<Result<bool>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product() { Name = request.Name };

        IDataService<Product> dataService = _dataServiceFactory.CreateService<Product>();
        dataService.Add(product);

        var result = await _dataServiceFactory.SaveChangesAsync(cancellationToken);
        return new Result<bool>() { Value = result > 0 };
    }
}

