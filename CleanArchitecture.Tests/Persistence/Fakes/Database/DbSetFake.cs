using CleanArchitecture.Domain.Persistence;

namespace CleanArchitecture.Tests.Persistence.Fakes.Database;

internal class DbSetFake<T> where T : IEntity
{
    public DbSetFake(T element, DbStateFake state)
    {
        Element = element;
        State = state;
    }

    public T Element { get; set; }
    public DbStateFake State { get; set; }
    public T? UpdatedElement { get; set; }
}
