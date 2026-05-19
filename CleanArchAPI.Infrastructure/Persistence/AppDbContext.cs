using CleanArchAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchAPI.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);
            e.Property(p => p.Price).HasColumnType("decimal(18,2)");
            e.HasOne(p => p.Category)
             .WithMany(c => c.Products)
             .HasForeignKey(p => p.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<AppUser>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.Username).IsRequired().HasMaxLength(50);
            e.HasIndex(u => u.Username).IsUnique();
        });

        // Seed-data
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics", Description = "Electronic devices" },
            new Category { Id = 2, Name = "Books", Description = "Physical and digital books" }
        );
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Price = 12999m, Stock = 10, CategoryId = 1 },
            new Product { Id = 2, Name = "Clean Code", Price = 299m, Stock = 50, CategoryId = 2 }
        );
    }
}