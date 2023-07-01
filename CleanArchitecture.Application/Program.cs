using Dapper;
using System.Reflection;
using CleanArchitecture.Application.Configuration;
using CleanArchitecture.Application.Core.Customers;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using CleanArchitecture.Application.Core.Products;
using CleanArchitecture.Application.Core.Products.Queries;
using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Persistence.Repositories;
using HealthChecks.UI.Client;

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

//builder.Services.AddMassTransit(_ =>
//{
//    _.AddConsumers(typeof(Program).Assembly);
//    _.AddConsumers(typeof(Program).Assembly);
//    _.UsingInMemory((context, cfg) =>
//    {
//        //cfg.UseSendFilter(typeof(TokenSendFilter<>), context);
//        //cfg.UsePublishFilter(typeof(TokenPublishFilter<>), context);
//        cfg.UseConsumeFilter(typeof(PerformanceBehaviour<>), context);

//        //cfg.ConfigureEndpoints(context);
//    });
//});

builder.Services.AddMediator(x =>
{
    x.AddConsumers(typeof(Program).Assembly);
});


var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

var dbSettings = configuration.GetSection("Database").Get<DatabaseSettings>();

if (dbSettings == null)
{
    throw new Exception("There is no db settings");
}

builder.Services.AddDbContext<EfDbContext>(options => options.UseSqlite(dbSettings.ConnectionString));
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
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

