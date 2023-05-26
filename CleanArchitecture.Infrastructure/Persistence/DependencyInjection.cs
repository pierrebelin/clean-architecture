using CleanArchitecture.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure.Persistence;

//public static class DependencyInjection
//{
//    // Mapster go further : https://code-maze.com/mapster-aspnetcore-introduction/
//    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
//    {
//        services = services
//                .AddSingleton(TypeAdapterConfig.GlobalSettings)
//                .AddScoped<IMapper, ServiceMapper>();

//        services.RegisterMapsterConfiguration();
//        return services;
//    }

//    public static void RegisterMapsterConfiguration(this IServiceCollection services)
//    {
//        TypeAdapterConfig<Product, Product>
//            .NewConfig()
//            .TwoWays();
//        //.Map(dest => dest.FullName, src => $"{src.Title} {src.FirstName} {src.LastName}");

//        TypeAdapterConfig<Customer, Customer>
//            .NewConfig()
//            .TwoWays();
//    }
//}