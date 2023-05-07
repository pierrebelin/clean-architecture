using CleanArchitecture.Domain.Persistence;

namespace CleanArchitecture.Tests.Fakes;

public class DataServiceFactoryFake : IDataServiceFactory
{
    private readonly Dictionary<Type, object> _dico = new();

    public IDataService<T> CreateService<T>() where T : class
    {
        if (!_dico.ContainsKey(typeof(T)))
        {
            _dico.Add(typeof(T), new DataServiceFake<T>());
        }
        return (IDataService<T>)_dico[typeof(T)];
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}