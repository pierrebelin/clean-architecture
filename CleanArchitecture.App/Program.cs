using Dapper;
using System.Reflection;
using CleanArchitecture.App.Controllers;
using CleanArchitecture.Application.Customers.Commands;
using CleanArchitecture.Application.Customers.Queries;
using CleanArchitecture.Application.HealthChecks;
using CleanArchitecture.Application.Products.Commands;
using CleanArchitecture.Application.Products.Queries;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using CleanArchitecture.App.Utils;

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

builder.Services.AddDbContext<EfDbContext>();
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
