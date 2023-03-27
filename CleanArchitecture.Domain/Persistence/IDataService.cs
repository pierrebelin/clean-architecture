using CleanArchitecture.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanArchitecture.Domain.Persistence
{
    public interface IDataService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> GetAll();
        T? GetFirstOfDefault(Expression<Func<T, bool>> predicate);
        void Add(T element);
    }
}