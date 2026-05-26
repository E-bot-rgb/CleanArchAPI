using AutoMapper;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Interfaces;
using MediatR;

namespace CleanArchAPI.Application.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(
    string? Name = null,
    int? CategoryId = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    int Page = 1,
    int PageSize = 10
) : IRequest<PagedResultDto<ProductDto>>;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, PagedResultDto<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetAllProductsHandler(IProductRepository repo, IMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<PagedResultDto<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken ct)
    {
        var (products, totalCount) = await _repo.GetFilteredAsync(
            request.Name,
            request.CategoryId,
            request.MinPrice,
            request.MaxPrice,
            request.Page,
            request.PageSize);

        return new PagedResultDto<ProductDto>
        {
            Items = _mapper.Map<IEnumerable<ProductDto>>(products),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}