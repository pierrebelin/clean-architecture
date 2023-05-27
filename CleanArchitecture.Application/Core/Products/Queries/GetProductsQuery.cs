using CleanArchitecture.Application.Mediator;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Core.Products.Queries;

public record GetProductsQuery() : IRequest<Result<IEnumerable<Product>, NotFound>>;