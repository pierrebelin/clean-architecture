using CleanArchitecture.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Domain.Persistence
{
    public interface IDataServiceFactory
    {
        IDataService<T> CreateService<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}