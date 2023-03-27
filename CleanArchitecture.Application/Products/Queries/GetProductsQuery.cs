using CleanArchitecture.Domain.DomainObjects;
using MediatR;

namespace CleanArchitecture.Application.Products.Queries;

public record GetProductsQuery() : IRequest<IEnumerable<Product>>;