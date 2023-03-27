using CleanArchitecture.Domain.DomainObjects;
using CleanArchitecture.Domain.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.Application.Products.Commands;

internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result>
{
    private readonly IDataServiceFactory _dataServiceFactory;

    public CreateProductCommandHandler(IDataServiceFactory dataServiceFactory)
    {
        _dataServiceFactory = dataServiceFactory;
    }

    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product() {Name = request.Name};

        IDataService<Product> dataService = _dataServiceFactory.CreateService<Product>();
        dataService.Add(product);

        await _dataServiceFactory.SaveChangesAsync(cancellationToken);

        return new Result() { IsSuccess = true };
    }
}

