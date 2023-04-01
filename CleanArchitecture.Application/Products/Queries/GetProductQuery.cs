using CleanArchitecture.Domain.DomainObjects;
using MediatR;

namespace CleanArchitecture.Application.Products.Queries;

public record GetProductQuery(Guid Id) : IRequest<Result<Product>>;