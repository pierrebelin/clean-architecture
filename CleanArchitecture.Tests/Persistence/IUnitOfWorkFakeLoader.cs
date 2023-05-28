using CleanArchitecture.Tests.Persistence.Fakes;

namespace CleanArchitecture.Tests.Persistence;

public interface IUnitOfWorkFakeLoader
{
    void LoadInto(UnitOfWorkFake unitOfWork);
}