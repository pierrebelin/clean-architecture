using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.DomainObjects;
using MediatR;

namespace CleanArchitecture.Application.UseCases.Products.Queries;

public record GetProductsQuery() : IRequest<Result<IEnumerable<Product>>>;