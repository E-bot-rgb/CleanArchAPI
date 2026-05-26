using CleanArchAPI.Domain.Entities;
using CleanArchAPI.Domain.Interfaces;
using CleanArchAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchAPI.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _ctx;
    public ProductRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetFilteredAsync(
        string? name,
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        int page,
        int pageSize)
    {
        var query = _ctx.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        var totalCount = await query.CountAsync();

        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount);
    }

    public async Task<Product?> GetByIdAsync(int id)
        => await _ctx.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product> CreateAsync(Product product)
    {
        _ctx.Products.Add(product);
        await _ctx.SaveChangesAsync();
        return (await GetByIdAsync(product.Id))!;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _ctx.Products.Update(product);
        await _ctx.SaveChangesAsync();
        return (await GetByIdAsync(product.Id))!;
    }

    public async Task DeleteAsync(int id)
    {
        var p = await _ctx.Products.FindAsync(id);
        if (p is not null) { _ctx.Products.Remove(p); await _ctx.SaveChangesAsync(); }
    }
}