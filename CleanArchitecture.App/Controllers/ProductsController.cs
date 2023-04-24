using CleanArchitecture.Application.Customers.Queries;
using CleanArchitecture.Application.Products.Commands;
using CleanArchitecture.Application.Products.Queries;
using MediatR;

namespace CleanArchitecture.App.Controllers
{
    public static class ProductsController
    {
        public static IEndpointRouteBuilder AddProductsMapEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("products", ProductsHandlers.GetProducts);
            endpoints.MapPost("products", ProductsHandlers.AddProduct);
            endpoints.MapGet("products/{id}", ProductsHandlers.GetProduct);
            return endpoints;
        }
    }

    public static class ProductsHandlers
    {
        public static async Task<IResult> GetProducts(IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetProductsQuery(), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }

        public static async Task<IResult> GetProduct(Guid id, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetProductQuery(id), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }

        public static async Task<IResult> AddProduct(CreateProductCommand product, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(product, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }
    }
}