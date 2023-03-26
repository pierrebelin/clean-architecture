using Dapper;
using System.Reflection;
using CleanArchitecture.Application.Products.Commands;
using CleanArchitecture.Application.Products.Queries;
using CleanArchitecture.Domain.Persistence;
using CleanArchitecture.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).GetTypeInfo().Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetProductsQuery).GetTypeInfo().Assembly);
});

builder.Services.AddDbContext<EfDbContext>(x =>
{
    x.UseSqlite(optionsBuilder => optionsBuilder.MigrationsAssembly("CleanArchitecture.Infrastructure"));
});

builder.Services.AddTransient<IEfDbContext, EfDbContext>();
builder.Services.AddTransient<IDapperDbContext, DapperDbContext>();

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
