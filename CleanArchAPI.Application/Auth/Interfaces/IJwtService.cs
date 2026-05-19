using CleanArchAPI.Domain.Entities;

namespace CleanArchAPI.Application.Auth.Interfaces;

public interface IJwtService
{
    string GenerateToken(AppUser user);
}