using CleanArchAPI.Domain.Entities;
using CleanArchAPI.Domain.Interfaces;
using CleanArchAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchAPI.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _ctx;
    public ProductRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Product>> GetAllAsync()
        => await _ctx.Products.Include(p => p.Category).ToListAsync();

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