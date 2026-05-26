using AutoMapper;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Interfaces;
using MediatR;

namespace CleanArchAPI.Application.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto?>;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly ICategoryRepository _repo;
    private readonly IMapper _mapper;

    public GetCategoryByIdHandler(ICategoryRepository repo, IMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken ct)
    {
        var category = await _repo.GetByIdAsync(request.Id);
        return category is null ? null : _mapper.Map<CategoryDto>(category);
    }
}