using AutoMapper;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Interfaces;
using MediatR;

namespace CleanArchAPI.Application.Categories.Queries.GetProductsByCategory;

public record GetProductsByCategoryQuery(int CategoryId) : IRequest<IEnumerable<ProductDto>?>;

public class GetProductsByCategoryHandler
    : IRequestHandler<GetProductsByCategoryQuery, IEnumerable<ProductDto>?>
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly IProductRepository _productRepo;
    private readonly IMapper _mapper;

    public GetProductsByCategoryHandler(
        ICategoryRepository categoryRepo,
        IProductRepository productRepo,
        IMapper mapper)
    {
        _categoryRepo = categoryRepo;
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>?> Handle(
        GetProductsByCategoryQuery request, CancellationToken ct)
    {
        var category = await _categoryRepo.GetByIdAsync(request.CategoryId);
        if (category is null) return null;

        var (products, _) = await _productRepo.GetFilteredAsync(
            name: null,
            categoryId: request.CategoryId,
            minPrice: null,
            maxPrice: null,
            page: 1,
            pageSize: 100);

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}