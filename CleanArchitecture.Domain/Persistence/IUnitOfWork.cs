namespace CleanArchitecture.Domain.Persistence
{
    public interface IUnitOfWork
    {
        //ICustomerRepository CustomerRepository { get; }
        //IProductRepository ProductRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}