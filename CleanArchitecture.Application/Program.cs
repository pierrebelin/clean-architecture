using Dapper;
using System.Reflection;
using CleanArchitecture.Application.Configuration;
using CleanArchitecture.Application.Core.Customers.Commands;
using CleanArchitecture.Application.HealthChecks;
using CleanArchitecture.Application.Core.Customers;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using CleanArchitecture.Application.Core.Products;
using CleanArchitecture.Application.Core.Products.Queries;
using CleanArchitecture.Application.Core.Customers.Queries;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddInfrastructure();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetProductsQuery).GetTypeInfo().Assembly);
});
builder.Services.AddMediator(x =>
{
    x.AddConsumersFromNamespaceContaining<GetCustomersQueryHandler>();
    x.AddConsumersFromNamespaceContaining<CreateCustomerCommandHandler>();
});


var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

var dbSettings = configuration.GetSection("Database").Get<DatabaseSettings>();
builder.Services.AddDbContext<EfDbContext>(options => options.UseSqlite(dbSettings.ConnectionString));
builder.Services.AddTransient<DbContext, EfDbContext>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();

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

