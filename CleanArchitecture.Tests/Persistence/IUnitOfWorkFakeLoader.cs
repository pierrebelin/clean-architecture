using CleanArchitecture.Tests.Persistence.Fakes;

namespace CleanArchitecture.Tests.Persistence;

internal interface IUnitOfWorkFakeLoader
{
    bool IsLoaded { get; }
    void LoadInto(UnitOfWorkFake unitOfWork, IServiceProvider serviceProvider);
}