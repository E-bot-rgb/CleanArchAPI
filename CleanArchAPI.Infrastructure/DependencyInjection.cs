using CleanArchAPI.Application.Auth.Interfaces;
using CleanArchAPI.Domain.Interfaces;
using CleanArchAPI.Infrastructure.Persistence;
using CleanArchAPI.Infrastructure.Repositories;
using CleanArchAPI.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}