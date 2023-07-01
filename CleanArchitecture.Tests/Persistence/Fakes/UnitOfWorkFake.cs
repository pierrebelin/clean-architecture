using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Tests.Persistence.Fakes.Database;

namespace CleanArchitecture.Tests.Persistence.Fakes;

internal class UnitOfWorkFake : IUnitOfWork
{
    private readonly IDbFake _db;

    public UnitOfWorkFake(
        IDbFake db,
        IServiceProvider serviceProvider,
        IUnitOfWorkFakeLoader loader)
    {
        _db = db;
        loader.LoadInto(this, serviceProvider);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_db.SaveChanges());
    }
}