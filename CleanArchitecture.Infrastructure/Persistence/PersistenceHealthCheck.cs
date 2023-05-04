using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.Infrastructure.Persistence;

public class PersistenceHealthCheck : IHealthCheck
{
    private readonly DbContext _dbContext;

    public PersistenceHealthCheck(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (_dbContext.Database.CanConnect())
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("Database is healthy"));
        }

        return Task.FromResult(
            new HealthCheckResult(
                context.Registration.FailureStatus, "Database is failing"));
    }
}