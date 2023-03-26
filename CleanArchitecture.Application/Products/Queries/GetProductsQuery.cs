using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Products.Queries;

public record GetProductsQuery() : IRequest<IEnumerable<Product>>;