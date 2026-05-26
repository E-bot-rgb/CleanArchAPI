using CleanArchAPI.Domain.Entities;

namespace CleanArchAPI.Domain.Interfaces;

public interface IProductRepository
{
    Task<(IEnumerable<Product> Products, int TotalCount)> GetFilteredAsync(
        string? name,
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        int page,
        int pageSize);
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task DeleteAsync(int id);
}