using AutoMapper;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CleanArchAPI.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(int Id, string Name, string? Description,
    decimal Price, int Stock, int CategoryId) : IRequest<ProductDto?>;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CategoryId).GreaterThan(0);
    }
}

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto?>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public UpdateProductHandler(IProductRepository repo, IMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<ProductDto?> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await _repo.GetByIdAsync(request.Id);
        if (product is null) return null;

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;

        return _mapper.Map<ProductDto>(await _repo.UpdateAsync(product));
    }
}