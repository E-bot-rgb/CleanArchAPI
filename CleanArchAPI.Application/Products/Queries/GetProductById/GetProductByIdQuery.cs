using AutoMapper;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Interfaces;
using MediatR;

namespace CleanArchAPI.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(IProductRepository repo, IMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await _repo.GetByIdAsync(request.Id);
        return product is null ? null : _mapper.Map<ProductDto>(product);
    }
}