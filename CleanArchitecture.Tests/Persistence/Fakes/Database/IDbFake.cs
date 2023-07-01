using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;

namespace CleanArchitecture.Tests.Persistence.Fakes.Database;

internal interface IDbFake
{
    List<DbSetFake<Customer>> Customers { get; }
    List<DbSetFake<Product>> Products { get; }

    void Add<T>(T entity) where T : IEntity;
    void Remove<T>(T entity) where T : IEntity;
    void Update<T>(T entity) where T : IEntity;

    int SaveChanges();
}