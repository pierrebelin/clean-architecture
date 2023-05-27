using CleanArchitecture.Application.Core.Products.Commands;
using CleanArchitecture.Application.Core.Products.Queries;
using MediatR;

namespace CleanArchitecture.Application.Core.Products
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
            return result.Match<IResult>(
                m => Results.Ok(m),
                failed => Results.NotFound());
        }

        public static async Task<IResult> GetProduct(Guid id, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetProductQuery(id), cancellationToken);
            return result.Match<IResult>(
                m => Results.Ok(m),
                failed => Results.NotFound());
        }

        public static async Task<IResult> AddProduct(CreateProductCommand product, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(product, cancellationToken);
            return result.Match<IResult>(
                m => Results.Ok(m),
                failed => Results.NotFound());
        }
    }
}