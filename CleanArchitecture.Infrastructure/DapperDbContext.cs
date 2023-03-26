using CleanArchitecture.Domain.Persistence;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CleanArchitecture.Infrastructure;

public class DapperDbContext : IDapperDbContext, IDisposable
{
    public SqliteConnection _dbConnection { get; }
    private readonly SqliteConnectionStringBuilder _connectionStringBuilder;
    private bool _disposedValue;

    public DapperDbContext()
    {
        _connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = "db.sqlite"
        };
        _dbConnection = new SqliteConnection(_connectionStringBuilder.ConnectionString);
        _dbConnection.Open();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _dbConnection.Close();
                _dbConnection.Dispose();
            }

            _disposedValue = true;
        }
    }


    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public IEnumerable<T> Query<T>(string sql)
    {
        return _dbConnection.Query<T>(sql);
    }
}