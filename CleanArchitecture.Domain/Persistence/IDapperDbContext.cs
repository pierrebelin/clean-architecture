using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Domain.Persistence
{
    public interface IDapperDbContext
    {
        IEnumerable<T> Query<T>(string sql);
    }
}