using AutoMapper;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Entities;
using CleanArchAPI.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CleanArchAPI.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(string Name, string? Description,
    decimal Price, int Stock, int CategoryId) : IRequest<ProductDto>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CategoryId).GreaterThan(0);
    }
}

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public CreateProductHandler(IProductRepository repo, IMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId
        };
        var created = await _repo.CreateAsync(product);
        return _mapper.Map<ProductDto>(created);
    }
}