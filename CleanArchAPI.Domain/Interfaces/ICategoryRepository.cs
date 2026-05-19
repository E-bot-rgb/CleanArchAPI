using CleanArchAPI.Domain.Entities;

namespace CleanArchAPI.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> CreateAsync(Category category);
    Task DeleteAsync(int id);
}