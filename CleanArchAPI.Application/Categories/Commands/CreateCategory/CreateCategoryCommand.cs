using AutoMapper;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Entities;
using CleanArchAPI.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CleanArchAPI.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Name, string? Description) : IRequest<CategoryDto>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
        => RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
}

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _repo;
    private readonly IMapper _mapper;

    public CreateCategoryHandler(ICategoryRepository repo, IMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        var category = new Category { Name = request.Name, Description = request.Description };
        return _mapper.Map<CategoryDto>(await _repo.CreateAsync(category));
    }
}