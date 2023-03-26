using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;
using MediatR;

namespace CleanArchitecture.Application.Products.Commands;

internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result>
{
    private readonly IEfDbContext _dbContext;


    public CreateProductCommandHandler(IEfDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name;
        Product product = new(Guid.NewGuid(), request.Name);

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result() { IsSuccess = true };
    }
}

