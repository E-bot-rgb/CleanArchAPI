using CleanArchAPI.Domain.Interfaces;
using MediatR;

namespace CleanArchAPI.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(int Id) : IRequest<bool>;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _repo;
    public DeleteCategoryHandler(ICategoryRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        if (await _repo.GetByIdAsync(request.Id) is null) return false;
        await _repo.DeleteAsync(request.Id);
        return true;
    }
}