using CleanArchitecture.Domain.Persistence;
using System.Linq.Expressions;

namespace CleanArchitecture.Tests.Fakes;

public class DataServiceFake<T> : IDataService<T> where T : class
{
    private readonly List<T> _data = new();
    public Task<IEnumerable<T>> GetAllAsync()
    {
        return Task.FromResult((IEnumerable<T>)_data);
    }

    public IEnumerable<T> GetAll()
    {
        return _data;
    }

    public T? GetFirstOfDefault(Expression<Func<T, bool>> predicate)
    {
        return _data.FirstOrDefault(predicate.Compile());
    }

    public void Add(T element)
    {
        _data.Add(element);
    }
}