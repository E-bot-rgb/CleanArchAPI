using CleanArchAPI.Domain.Entities;
using CleanArchAPI.Domain.Interfaces;
using CleanArchAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchAPI.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _ctx;
    public CategoryRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Category>> GetAllAsync()
        => await _ctx.Categories.Include(c => c.Products).ToListAsync();

    public async Task<Category?> GetByIdAsync(int id)
        => await _ctx.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Category> CreateAsync(Category category)
    {
        _ctx.Categories.Add(category);
        await _ctx.SaveChangesAsync();
        return category;
    }

    public async Task DeleteAsync(int id)
    {
        var c = await _ctx.Categories.FindAsync(id);
        if (c is not null) { _ctx.Categories.Remove(c); await _ctx.SaveChangesAsync(); }
    }
}