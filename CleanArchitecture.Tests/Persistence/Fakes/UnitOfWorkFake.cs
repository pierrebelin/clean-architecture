using CleanArchitecture.Domain.Persistence;

namespace CleanArchitecture.Tests.Persistence.Fakes;

public class UnitOfWorkFake : IUnitOfWork
{
    public ICustomerRepository CustomerRepository { get; }
    public IProductRepository ProductRepository { get; }

    public UnitOfWorkFake(IUnitOfWorkFakeLoader loader,
        ICustomerRepository customerRepository,
        IProductRepository productRepository)
    {
        CustomerRepository = customerRepository;
        ProductRepository = productRepository;

        loader.LoadInto(this);
    }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => 1, cancellationToken);
    }
}