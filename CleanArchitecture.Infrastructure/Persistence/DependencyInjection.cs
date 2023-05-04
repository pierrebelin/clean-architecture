﻿using Mapster;
using Microsoft.Extensions.DependencyInjection;
using MapsterMapper;
using CleanArchitecture.Infrastructure.Persistence.Entities;

namespace CleanArchitecture.Infrastructure.Persistence;

public static class DependencyInjection
{
    // Mapster go further : https://code-maze.com/mapster-aspnetcore-introduction/
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services = services
                .AddSingleton(TypeAdapterConfig.GlobalSettings)
                .AddScoped<IMapper, ServiceMapper>();

        services.RegisterMapsterConfiguration();
        return services;
    }

    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Product, Domain.DomainObjects.Product>
            .NewConfig()
            .TwoWays();
        //.Map(dest => dest.FullName, src => $"{src.Title} {src.FirstName} {src.LastName}");

        TypeAdapterConfig<Customer, Domain.DomainObjects.Customer>
            .NewConfig()
            .TwoWays();
    }
}