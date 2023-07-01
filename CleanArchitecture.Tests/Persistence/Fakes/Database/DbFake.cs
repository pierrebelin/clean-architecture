using System.Collections.Concurrent;
using System.Reflection;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Persistence;

namespace CleanArchitecture.Tests.Persistence.Fakes.Database;

internal class DbFake : IDbFake
{
    private readonly object _lock = new();

    public List<DbSetFake<Customer>> Customers { get; } = new();
    public List<DbSetFake<Product>> Products { get; } = new();

    public int SaveChanges()
    {
        lock (_lock)
        {
            UpdateDbSets();
            return 1;
        }
    }

    // TODO: Improve this by using reflection instead of hardcoding
    private void UpdateDbSets()
    {
        UpdateDbSet(Customers);
        UpdateDbSet(Products);

        //foreach (var propertyInfo in GetAllDbSets())
        //{
        //    var dbSet = (ConcurrentBag<DbSetFake<IEntity>>)propertyInfo.GetValue(this, null);
        //    UpdateDbSet(dbSet);
        //}
    }

    public void Add<T>(T entity) where T : IEntity
    {
        var propertyInfo = GetAllDbSets()
            .FirstOrDefault(_ => _.PropertyType.GenericTypeArguments.First().GenericTypeArguments.First().Name == typeof(T).Name);
        var dbSet = (List<DbSetFake<T>>)propertyInfo.GetValue(this, null);
        dbSet.Add(new DbSetFake<T>(entity, DbStateFake.Added));
    }

    public void Remove<T>(T entity) where T: IEntity
    {
        var propertyInfo = GetAllDbSets()
            .FirstOrDefault(_ => _.PropertyType.GenericTypeArguments.First().GenericTypeArguments.First().Name == typeof(T).Name);
        var dbSet = (List<DbSetFake<T>>)propertyInfo.GetValue(this, null);
        var elementToRemove = dbSet.FirstOrDefault(_ => _.Element.Id == entity.Id);
        if (elementToRemove == null)
        {
            throw new Exception();
        }
        elementToRemove.State = DbStateFake.Deleted;
    }

    public void Update<T>(T entity) where T : IEntity
    {
        var propertyInfo = GetAllDbSets()
            .FirstOrDefault(_ => _.PropertyType.GenericTypeArguments.First().GenericTypeArguments.First().Name == typeof(T).Name);
        var dbSet = (List<DbSetFake<T>>)propertyInfo.GetValue(this, null);
        var elementToUpdate = dbSet.FirstOrDefault(_ => _.Element.Id == entity.Id);
        if (elementToUpdate == null)
        {
            throw new Exception();
        }
        elementToUpdate.State = DbStateFake.Modified;
        elementToUpdate.UpdatedElement = entity;
    }

    private PropertyInfo[] GetAllDbSets()
    {
        return GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    private void UpdateDbSet<T>(List<DbSetFake<T>> dbSet) where T : IEntity
    {
        dbSet.Where(_ => _.State == DbStateFake.Added).ToList().ForEach(_ => _.State = DbStateFake.Ok);
        dbSet.Where(_ => _.State == DbStateFake.Modified).ToList().ForEach(_ =>
        {
            _.Element = _.UpdatedElement;
            _.State = DbStateFake.Ok;
            _.UpdatedElement = default;
        });
        dbSet.RemoveAll(_ => dbSet.Where(__ => __.State == DbStateFake.Deleted).ToList().Contains(_));
    }
}