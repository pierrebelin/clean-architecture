using CleanArchitecture.Domain.Persistence;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure.Persistence;

public class DataService<T, R> : IDataService<T> where T : class where R : class
{
    private readonly DbContext _dbContext;

    public DataService(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        var result = await _dbContext.Set<R>().ProjectToType<T>().ToArrayAsync();
        return result;
    }

    public virtual IEnumerable<T> GetAll()
    {
        var result = _dbContext.Set<R>().ProjectToType<T>().ToArray();
        return result;
    }

    public virtual T? GetFirstOfDefault(Expression<Func<T, bool>> predicate)
    {
        var predicateDto = Map(predicate.Compile(), (R x) => x.Adapt<T>());
        var result = _dbContext.Set<R>().Where(predicateDto).FirstOrDefault();
        return result?.Adapt<T?>();
    }

    public void Add(T element)
    {
        _dbContext.Set<R>().Add(element.Adapt<R>());
    }

    private static Func<TNewIn, TOut> Map<TOrigIn, TNewIn, TOut>(Func<TOrigIn, TOut> input,
        Func<TNewIn, TOrigIn> convert)
    {
        return x => input(convert(x));
    }
}