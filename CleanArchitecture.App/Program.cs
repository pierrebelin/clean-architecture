using Dapper;
using System.Reflection;
using CleanArchitecture.Application.Customers.Commands;
using CleanArchitecture.Application.Customers.Queries;
using CleanArchitecture.Application.Products.Commands;
using CleanArchitecture.Application.Products.Queries;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MassTransit;

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

SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
SqlMapper.RemoveTypeMap(typeof(Guid));
SqlMapper.RemoveTypeMap(typeof(Guid?));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
