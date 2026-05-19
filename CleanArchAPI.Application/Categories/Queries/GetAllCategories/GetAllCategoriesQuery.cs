using AutoMapper;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Interfaces;
using MediatR;

namespace CleanArchAPI.Application.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _repo;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(ICategoryRepository repo, IMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken ct)
        => _mapper.Map<IEnumerable<CategoryDto>>(await _repo.GetAllAsync());
}