using AutoMapper;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Interfaces;
using MediatR;

namespace CleanArchAPI.Application.Products.Queries.GetAllProducts;

public record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetAllProductsHandler(IProductRepository repo, IMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken ct)
        => _mapper.Map<IEnumerable<ProductDto>>(await _repo.GetAllAsync());
}