using System.Reflection;
using CleanArchitecture.Domain.DomainObjects;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Infrastructure.Persistence.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence;

public class DataServiceFactory : IDataServiceFactory
{
    private readonly DbContext _dbContext;

    public DataServiceFactory(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private readonly Dictionary<Type, Type> _mappingDomainObjectsToDto = new()
    {
        {typeof(Domain.DomainObjects.Product), typeof(Entities.Product)},
        {typeof(Domain.DomainObjects.Customer), typeof(Entities.Customer)}
    };

    public IDataService<T>? CreateService<T>() where T : class
    {
        var dtoType = _mappingDomainObjectsToDto[typeof(T)];

        var d1 = typeof(DataService<,>);
        Type[] typeArgs = { typeof(T), dtoType };
        var makeme = d1.MakeGenericType(typeArgs);
        var o = Activator.CreateInstance(makeme, _dbContext);
        var dataService = o as IDataService<T>;

        return dataService;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}