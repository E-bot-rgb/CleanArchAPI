using CleanArchAPI.Domain.Interfaces;
using MediatR;

namespace CleanArchAPI.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(int Id) : IRequest<bool>;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _repo;
    public DeleteProductHandler(IProductRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken ct)
    {
        if (await _repo.GetByIdAsync(request.Id) is null) return false;
        await _repo.DeleteAsync(request.Id);
        return true;
    }
}