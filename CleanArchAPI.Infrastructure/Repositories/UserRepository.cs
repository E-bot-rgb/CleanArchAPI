using CleanArchAPI.Domain.Entities;
using CleanArchAPI.Domain.Interfaces;
using CleanArchAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _ctx;
    public UserRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<AppUser?> GetByUsernameAsync(string username)
        => await _ctx.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<AppUser> CreateAsync(AppUser user)
    {
        _ctx.Users.Add(user);
        await _ctx.SaveChangesAsync();
        return user;
    }
}