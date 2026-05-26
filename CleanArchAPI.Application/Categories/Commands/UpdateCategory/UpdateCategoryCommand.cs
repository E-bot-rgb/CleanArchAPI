using AutoMapper;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CleanArchAPI.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(int Id, string Name, string? Description) : IRequest<CategoryDto?>;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto?>
{
    private readonly ICategoryRepository _repo;
    private readonly IMapper _mapper;

    public UpdateCategoryHandler(ICategoryRepository repo, IMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<CategoryDto?> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var category = await _repo.GetByIdAsync(request.Id);
        if (category is null) return null;

        category.Name = request.Name;
        category.Description = request.Description;

        var updated = await _repo.UpdateAsync(category);
        return _mapper.Map<CategoryDto>(updated);
    }
}