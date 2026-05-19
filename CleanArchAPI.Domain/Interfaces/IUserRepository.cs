using CleanArchAPI.Domain.Entities;

namespace CleanArchAPI.Domain.Interfaces;

public interface IUserRepository
{
    Task<AppUser?> GetByUsernameAsync(string username);
    Task<AppUser> CreateAsync(AppUser user);
}