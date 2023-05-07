using Dapper;
using System.Reflection;
using CleanArchitecture.Application.Configuration;
using CleanArchitecture.Application.HealthChecks;
using CleanArchitecture.Application.UseCases.Customers;
using CleanArchitecture.Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using CleanArchitecture.Application.UseCases.Products;
using CleanArchitecture.Application.UseCases.Products.Queries;
using CleanArchitecture.Application.UseCases.Customers.Commands;
using CleanArchitecture.Application.UseCases.Customers.Queries;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetProductsQuery).GetTypeInfo().Assembly);
});
builder.Services.AddMediator(x =>
{
    x.AddConsumersFromNamespaceContaining<GetCustomersConsumer>();
    x.AddConsumersFromNamespaceContaining<CreateCustomerConsumer>();
});


var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

var dbSettings = configuration.GetSection("Database").Get<DatabaseSettings>();
builder.Services.AddDbContext<EfDbContext>(options => options.UseSqlite(dbSettings.ConnectionString));
builder.Services.AddTransient<DbContext, EfDbContext>();
builder.Services.AddTransient<IDataServiceFactory, DataServiceFactory>();

builder.Services.AddHealthChecks()
    .AddCheck<PersistenceHealthCheck>(nameof(PersistenceHealthCheck));

SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
SqlMapper.RemoveTypeMap(typeof(Guid));
SqlMapper.RemoveTypeMap(typeof(Guid?));

var app = builder.Build();

app.AddCustomersMapEndpoints();
app.AddProductsMapEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = HealthCheckExtensions.WriteHealthCheckResponse
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

